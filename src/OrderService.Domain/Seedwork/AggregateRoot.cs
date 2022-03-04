using System;
using System.Collections.Generic;

namespace OrderService.Domain.Seedwork
{
    public abstract class AggregateRoot
    {
        private readonly List<IDomainEvent> _domainEvents = new();

        public virtual Guid Id { get; protected set; }
        public virtual IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;
        protected AggregateRoot() { }
        protected AggregateRoot(Guid id)
        {
            Id = id;
        }

        protected virtual void AddDomainEvent(IDomainEvent newEvent)
        {
            _domainEvents.Add(newEvent);
        }

        public virtual void ClearEvents()
        {
            _domainEvents.Clear();
        }
    }
}
