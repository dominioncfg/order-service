using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain;

namespace OrderService.Infrastructure;

internal class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable(EntityFrameworkConfigurationConstants.Orders, EntityFrameworkConfigurationConstants.MainSchema);

        builder.HasKey(m => m.Id);
        builder.HasMany(m => m.Items).WithOne().HasForeignKey(EntityFrameworkConfigurationConstants.ShadowFields.OrderId);
        builder.Ignore(m => m.DomainEvents);
    }
}
