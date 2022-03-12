using OrderService.Domain.Seedwork;

namespace OrderService.Api.FunctionalTests.SeedWork;
internal class MockClockService : IClockService
{
    private static readonly Lazy<MockClockService> _service;
    private static DateTime _utcNow = DateTime.MinValue;
    public DateTime UtcNow => _utcNow;
    public static MockClockService Service => _service.Value;
    private MockClockService() { }
    static MockClockService()
    {
        _service = new Lazy<MockClockService>(() => new MockClockService());
    }

    public static void AssumeUtcNowAs(DateTime nowUtcDateTime) => _utcNow = nowUtcDateTime;
    public static void ResetService() => _utcNow = DateTime.MinValue;
}
