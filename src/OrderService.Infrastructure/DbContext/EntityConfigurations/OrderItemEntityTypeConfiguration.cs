using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OrderService.Infrastructure;

internal class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable(EntityFrameworkConfigurationConstants.OrdersItems, EntityFrameworkConfigurationConstants.MainSchema);
        builder.HasKey(x => x.Id);

        builder
           .Property(m => m.Sku)
           .HasConversion(prop => prop.Value, value => new OrderItemSku(value))
           .IsRequired();

        builder
            .Property(m => m.UnitPrice)
            .HasConversion(prop => prop.PriceInEuros, value => OrderItemUnitPrice.FromEuros(value))
            .IsRequired();

        builder
           .Property(m => m.Quantity)
           .HasConversion(prop => prop.Value, value => new OrderItemQuantity(value))
           .IsRequired();
    }
}
