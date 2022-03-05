namespace OrderService.Api.Configuration;

internal record RabbitMqOptions
{
    public const string Section = "RabbitMq";
    public string Host { get; init; }
    public string UserName { get; init; }
    public string Password { get; init; }
}
