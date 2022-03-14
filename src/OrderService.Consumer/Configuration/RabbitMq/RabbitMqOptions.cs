namespace OrderService.Consumer.Configuration;

internal record RabbitMqOptions
{
    public const string Section = "RabbitMq";
    public string Host { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string QueueName { get; init; } = string.Empty;
}
