using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain;

namespace OrderService.Infrastructure;

internal class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable(EntityFrameworkConfigurationConstants.OrdersItems, EntityFrameworkConfigurationConstants.MainSchema);
        builder.HasKey(EntityFrameworkConfigurationConstants.ShadowFields.OrderId, nameof(OrderItem.Sku));
        builder.Property(x => x.Quantity);
    }
}
