using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Shared.Behaviors;

/// <summary>
/// Pipeline behavior que loga todas as requisições
/// </summary>
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var correlationId = Activity.Current?.Id ?? Guid.NewGuid().ToString();

        _logger.LogInformation(
            "Handling {RequestName} [{CorrelationId}]",
            requestName,
            correlationId
        );

        try
        {
            var response = await next();

            _logger.LogInformation(
                "Handled {RequestName} [{CorrelationId}]",
                requestName,
                correlationId
            );

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error handling {RequestName} [{CorrelationId}]: {ErrorMessage}",
                requestName,
                correlationId,
                ex.Message
            );
            throw;
        }
    }
}
