namespace OrderService.Api.FunctionalTests.SeedWork;

public static class EventsExtensions
{
    public static List<T> GetPublishedEventsOfType<T>(this TestServerFixture given) where T : class
    {
        var events = given.CurrentTestPublishedEvents!.Messages.Select<T>();
        var eventMessages = events.Select(x => x.Context.Message).ToList();
        return eventMessages;
    }
}
