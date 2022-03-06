namespace OrderService.Infrastructure;

public static class EntityFrameworkConfigurationConstants
{
    public static readonly string MainSchema = "core";

    public static readonly string Orders = "orders";
    public static readonly string OrdersItems = "order_items";

    public static class ShadowFields
    {
        public static readonly string OrderId = "order_id";
    }
}
