using OrderService.Api.FunctionalTests.SeedWork;
using System.Collections.Generic;
using System.Linq;

namespace OrderService.Api.FunctionalTests.Shared
{
    public static class EventsExtensions
    {
        public static List<T> GetPublishedEventsOfType<T>(this TestServerFixture fixture) where T : class
        {
            var events = TestServerFixture.CurrentTestPublishedEvents.Messages.Select<T>();
            var eventMessages = events.Select(x => x.Context.Message).ToList();
            return eventMessages;
        }
    }
}
