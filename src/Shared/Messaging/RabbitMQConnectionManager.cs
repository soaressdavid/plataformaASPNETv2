using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using Shared.Interfaces;
using System.Collections.Concurrent;

namespace Shared.Messaging;

/// <summary>
/// Manages RabbitMQ connections and channels with automatic reconnection, retry logic, and channel pooling
/// </summary>
public class RabbitMQConnectionManager : IRabbitMQConnectionManager
{
    private readonly RabbitMQOptions _options;
    private readonly ILogger<RabbitMQConnectionManager> _logger;
    private readonly ResiliencePipeline _retryPipeline;
    private readonly SemaphoreSlim _connectionLock = new(1, 1);
    private readonly ConcurrentBag<IModel> _channelPool = new();
    private IConnection? _connection;
    private bool _disposed;

    public RabbitMQConnectionManager(
        IOptions<RabbitMQOptions> options,
        ILogger<RabbitMQConnectionManager> logger)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        _options = options.Value ?? throw new ArgumentNullException(nameof(options), "RabbitMQ options value cannot be null");
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        // Configure retry policy with exponential backoff
        _retryPipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = _options.MaxRetryAttempts,
                Delay = TimeSpan.FromMilliseconds(_options.RetryDelayMs),
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                OnRetry = args =>
                {
                    _logger.LogWarning(
                        "RabbitMQ operation failed. Attempt {Attempt} of {MaxAttempts}. Retrying in {Delay}ms. Error: {Error}",
                        args.AttemptNumber + 1,
                        _options.MaxRetryAttempts,
                        args.RetryDelay.TotalMilliseconds,
                        args.Outcome.Exception?.Message);
                    return ValueTask.CompletedTask;
                },
                ShouldHandle = new PredicateBuilder().Handle<BrokerUnreachableException>()
                    .Handle<OperationInterruptedException>()
                    .Handle<TimeoutException>()
            })
            .Build();

        _logger.LogInformation("RabbitMQ Connection Manager initialized");
    }

    public bool IsConnected => _connection?.IsOpen ?? false;

    public IConnection GetConnection()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(RabbitMQConnectionManager));
        }

        if (_connection != null && _connection.IsOpen)
        {
            return _connection;
        }

        _connectionLock.Wait();
        try
        {
            // Double-check after acquiring lock
            if (_connection != null && _connection.IsOpen)
            {
                return _connection;
            }

            _logger.LogInformation("Creating new RabbitMQ connection to {Host}:{Port}", _options.HostName, _options.Port);

            var factory = new ConnectionFactory
            {
                HostName = _options.HostName,
                Port = _options.Port,
                UserName = _options.UserName,
                Password = _options.Password,
                VirtualHost = _options.VirtualHost,
                RequestedHeartbeat = TimeSpan.FromSeconds(_options.RequestedHeartbeat),
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
                TopologyRecoveryEnabled = true,
                DispatchConsumersAsync = true
            };

            _connection = _retryPipeline.Execute(() =>
            {
                var conn = factory.CreateConnection();
                _logger.LogInformation("Successfully connected to RabbitMQ at {Host}:{Port}", _options.HostName, _options.Port);
                return conn;
            });

            // Set up connection event handlers
            _connection.ConnectionShutdown += OnConnectionShutdown;
            _connection.CallbackException += OnCallbackException;
            _connection.ConnectionBlocked += OnConnectionBlocked;
            _connection.ConnectionUnblocked += OnConnectionUnblocked;

            return _connection;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create RabbitMQ connection after {MaxAttempts} attempts", _options.MaxRetryAttempts);
            throw;
        }
        finally
        {
            _connectionLock.Release();
        }
    }

    public IModel CreateChannel()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(RabbitMQConnectionManager));
        }

        // Try to reuse a channel from the pool
        if (_channelPool.TryTake(out var pooledChannel) && pooledChannel.IsOpen)
        {
            _logger.LogDebug("Reusing channel from pool");
            return pooledChannel;
        }

        var connection = GetConnection();
        var channel = _retryPipeline.Execute(() =>
        {
            var ch = connection.CreateModel();
            _logger.LogDebug("Created new RabbitMQ channel");
            return ch;
        });

        // Set up channel event handlers
        channel.ModelShutdown += OnChannelShutdown;
        channel.CallbackException += OnChannelCallbackException;

        return channel;
    }

    public async Task DeclareExchangeAsync(string exchangeName, string exchangeType, bool durable = true, bool autoDelete = false)
    {
        if (string.IsNullOrWhiteSpace(exchangeName))
        {
            throw new ArgumentException("Exchange name cannot be null or empty", nameof(exchangeName));
        }

        if (string.IsNullOrWhiteSpace(exchangeType))
        {
            throw new ArgumentException("Exchange type cannot be null or empty", nameof(exchangeType));
        }

        using var channel = CreateChannel();
        
        await _retryPipeline.ExecuteAsync(async cancellationToken =>
        {
            await Task.Run(() =>
            {
                channel.ExchangeDeclare(
                    exchange: exchangeName,
                    type: exchangeType,
                    durable: durable,
                    autoDelete: autoDelete,
                    arguments: null);

                _logger.LogInformation(
                    "Declared exchange '{ExchangeName}' of type '{ExchangeType}' (durable: {Durable}, autoDelete: {AutoDelete})",
                    exchangeName, exchangeType, durable, autoDelete);
            }, cancellationToken);
        }, CancellationToken.None);
    }

    public async Task DeclareQueueAsync(string queueName, bool durable = true, bool exclusive = false, bool autoDelete = false)
    {
        if (string.IsNullOrWhiteSpace(queueName))
        {
            throw new ArgumentException("Queue name cannot be null or empty", nameof(queueName));
        }

        using var channel = CreateChannel();
        
        await _retryPipeline.ExecuteAsync(async cancellationToken =>
        {
            await Task.Run(() =>
            {
                channel.QueueDeclare(
                    queue: queueName,
                    durable: durable,
                    exclusive: exclusive,
                    autoDelete: autoDelete,
                    arguments: null);

                _logger.LogInformation(
                    "Declared queue '{QueueName}' (durable: {Durable}, exclusive: {Exclusive}, autoDelete: {AutoDelete})",
                    queueName, durable, exclusive, autoDelete);
            }, cancellationToken);
        }, CancellationToken.None);
    }

    public async Task BindQueueAsync(string queueName, string exchangeName, string routingKey)
    {
        if (string.IsNullOrWhiteSpace(queueName))
        {
            throw new ArgumentException("Queue name cannot be null or empty", nameof(queueName));
        }

        if (string.IsNullOrWhiteSpace(exchangeName))
        {
            throw new ArgumentException("Exchange name cannot be null or empty", nameof(exchangeName));
        }

        if (string.IsNullOrWhiteSpace(routingKey))
        {
            throw new ArgumentException("Routing key cannot be null or empty", nameof(routingKey));
        }

        using var channel = CreateChannel();
        
        await _retryPipeline.ExecuteAsync(async cancellationToken =>
        {
            await Task.Run(() =>
            {
                channel.QueueBind(
                    queue: queueName,
                    exchange: exchangeName,
                    routingKey: routingKey,
                    arguments: null);

                _logger.LogInformation(
                    "Bound queue '{QueueName}' to exchange '{ExchangeName}' with routing key '{RoutingKey}'",
                    queueName, exchangeName, routingKey);
            }, cancellationToken);
        }, CancellationToken.None);
    }

    private void OnConnectionShutdown(object? sender, ShutdownEventArgs args)
    {
        if (_disposed) return;

        _logger.LogWarning(
            "RabbitMQ connection shutdown. ReplyCode: {ReplyCode}, ReplyText: {ReplyText}",
            args.ReplyCode, args.ReplyText);

        // Clear channel pool on connection shutdown
        while (_channelPool.TryTake(out var channel))
        {
            try
            {
                channel.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error disposing channel from pool");
            }
        }
    }

    private void OnCallbackException(object? sender, CallbackExceptionEventArgs args)
    {
        _logger.LogError(args.Exception, "RabbitMQ connection callback exception: {Detail}", args.Detail);
    }

    private void OnConnectionBlocked(object? sender, ConnectionBlockedEventArgs args)
    {
        _logger.LogWarning("RabbitMQ connection blocked: {Reason}", args.Reason);
    }

    private void OnConnectionUnblocked(object? sender, EventArgs args)
    {
        _logger.LogInformation("RabbitMQ connection unblocked");
    }

    private void OnChannelShutdown(object? sender, ShutdownEventArgs args)
    {
        _logger.LogWarning(
            "RabbitMQ channel shutdown. ReplyCode: {ReplyCode}, ReplyText: {ReplyText}",
            args.ReplyCode, args.ReplyText);
    }

    private void OnChannelCallbackException(object? sender, CallbackExceptionEventArgs args)
    {
        _logger.LogError(args.Exception, "RabbitMQ channel callback exception: {Detail}", args.Detail);
    }

    public void Dispose()
    {
        if (_disposed) return;

        _disposed = true;

        _logger.LogInformation("Disposing RabbitMQ Connection Manager");

        // Dispose all channels in the pool
        while (_channelPool.TryTake(out var channel))
        {
            try
            {
                channel.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error disposing channel from pool");
            }
        }

        // Dispose connection
        if (_connection != null)
        {
            try
            {
                _connection.ConnectionShutdown -= OnConnectionShutdown;
                _connection.CallbackException -= OnCallbackException;
                _connection.ConnectionBlocked -= OnConnectionBlocked;
                _connection.ConnectionUnblocked -= OnConnectionUnblocked;

                _connection.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error disposing RabbitMQ connection");
            }
        }

        _connectionLock.Dispose();

        _logger.LogInformation("RabbitMQ Connection Manager disposed");
    }
}
