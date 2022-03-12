using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Orders;

namespace OrderService.Infrastructure;

internal class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable(EntityFrameworkConfigurationConstants.Orders, EntityFrameworkConfigurationConstants.MainSchema);
        builder.HasKey(m => m.Id);
        builder
            .HasMany(m => m.Items)
            .WithOne()
            .HasForeignKey(EntityFrameworkConfigurationConstants.ShadowFields.OrderId)
            .IsRequired();
        builder.Ignore(m => m.DomainEvents);

        builder.Property(m => m.BuyerId).IsRequired();
        builder
            .Property(m => m.CreationDateTime)
            .HasConversion(prop => prop.UtcValue, value => OrderCreationDate.FromUtc(value))
            .IsRequired();

        builder.OwnsOne(m => m.Status);
        builder.OwnsOne(m => m.Address, addressBuilder =>
        {
            addressBuilder.Property(address => address.Country).IsRequired();
            addressBuilder.Property(address => address.City).IsRequired();
            addressBuilder.Property(address => address.Street).IsRequired();
            addressBuilder.Property(address => address.Number).IsRequired();
        });

    }
}
