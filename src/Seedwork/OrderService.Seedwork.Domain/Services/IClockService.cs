namespace OrderService.Seedwork.Domain;

public interface IClockService
{
    DateTime UtcNow { get; }
}
