namespace OrderService.Domain.Seedwork;

public interface IClockService
{
    DateTime UtcNow { get; }
}
