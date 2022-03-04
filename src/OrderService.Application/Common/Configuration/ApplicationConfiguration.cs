using System.Reflection;

namespace OrderService.Application.Common.Configuration
{
    public static class ApplicationConfiguration
    {
        public static Assembly GetApplicationAssembly() => Assembly.GetExecutingAssembly();
    }
}
