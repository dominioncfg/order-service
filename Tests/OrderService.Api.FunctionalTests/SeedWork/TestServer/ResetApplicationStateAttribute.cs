using System.Reflection;
using Xunit.Sdk;

namespace OrderService.Api.FunctionalTests.SeedWork
{
    public class ResetApplicationStateAttribute : BeforeAfterTestAttribute
    {
        public override void After(MethodInfo methodUnderTest) => base.After(methodUnderTest);
        public override void Before(MethodInfo methodUnderTest) => TestServerFixture.OnTestInitResetApplicationState();
    }
}
