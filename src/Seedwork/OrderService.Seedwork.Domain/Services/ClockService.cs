namespace OrderService.Seedwork.Domain;

public class ClockService : IClockService
{
    public DateTime UtcNow => DateTime.UtcNow;
}
