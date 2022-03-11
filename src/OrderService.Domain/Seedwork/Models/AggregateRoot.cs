namespace OrderService.Domain.Seedwork;

public abstract class AggregateRoot<TId> : Entity<TId>
{
    private readonly List<IDomainEvent> _domainEvents = new();
    public virtual IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;
    protected AggregateRoot() : base() { }
    protected AggregateRoot(TId id) : base(id) { }

    protected virtual void AddDomainEvent(IDomainEvent newEvent)
    {
        _domainEvents.Add(newEvent);
    }

    public virtual void ClearEvents()
    {
        _domainEvents.Clear();
    }
}
