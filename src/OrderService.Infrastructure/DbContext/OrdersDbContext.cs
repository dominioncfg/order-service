using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Orders;
using OrderService.Seedwork.Domain;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Infrastructure;

public class OrdersDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public readonly IMediator _mediator;

#nullable disable
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }
#nullable enable

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        AddEntitiesConfiguration(modelBuilder);
    }

    private static void AddEntitiesConfiguration(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemEntityTypeConfiguration());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        await DispatchDomainEventsForTrackedEntities();
        return result;
    }

    public override int SaveChanges()
    {
        return SaveChangesAsync(default).GetAwaiter().GetResult();
    }

    public async Task ClearDatabaseBeforeTestAsync()
    {
        Orders.ToList().ForEach(x => Orders.Remove(x));
        await base.SaveChangesAsync(default);
    }

    private async Task DispatchDomainEventsForTrackedEntities()
    {
        if (_mediator == null)
            throw new NullReferenceException("Mediator is required");

        var entitiesWithEvents = ChangeTracker.Entries<AggregateRoot<Guid>>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .ToArray();

        foreach (var entity in entitiesWithEvents)
        {
            var events = entity.DomainEvents.ToArray();
            entity.ClearEvents();
            foreach (var domainEvent in events)
            {
                await _mediator.Publish(domainEvent).ConfigureAwait(false);
            }
        }
    }
}
