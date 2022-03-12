namespace OrderService.Seedwork.Domain;

public interface IAggregateRepository<T, Tid> where T : AggregateRoot<Tid>
{
    Task AddAsync(T aggregateRoot, CancellationToken cancellationToken);   
}