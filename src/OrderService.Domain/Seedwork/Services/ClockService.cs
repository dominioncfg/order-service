namespace OrderService.Domain.Seedwork;

public class ClockService : IClockService
{
    public DateTime UtcNow => DateTime.UtcNow;
}
