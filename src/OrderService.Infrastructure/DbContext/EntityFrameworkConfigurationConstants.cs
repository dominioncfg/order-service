namespace OrderService.Infrastructure
{
    public static class EntityFrameworkConfigurationConstants
    {
        public static readonly string MainSchema = "Domain";

        public static readonly string Orders = nameof(Orders);
        public static readonly string OrdersItems = nameof(OrdersItems);

        public static class ShadowFields
        {
            public static readonly string OrderId = nameof(OrderId);
        }
    }
}
