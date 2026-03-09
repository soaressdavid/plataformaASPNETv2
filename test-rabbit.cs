using System;
using RabbitMQ.Client;

var factory = new ConnectionFactory
{
    HostName = "localhost",
    Port = 5672,
    UserName = "platform_user",
    Password = "SimplePass123",
    VirtualHost = "/",
    RequestedConnectionTimeout = TimeSpan.FromSeconds(5)
};

try
{
    using var connection = factory.CreateConnection();
    Console.WriteLine($"Connected successfully! IsOpen: {connection.IsOpen}");
}
catch (Exception ex)
{
    Console.WriteLine($"Connection failed: {ex.Message}");
}
