namespace OrderService.Api.FunctionalTests.SeedWork;

public static class MocksFixtureExtensions
{
    public static void AssumeClockUtcNowAt(this TestServerFixture _, DateTime utcNow)
    {
        MockClockService.AssumeUtcNowAs(utcNow);
    }
}