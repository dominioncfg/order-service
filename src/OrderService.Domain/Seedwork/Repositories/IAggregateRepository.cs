namespace OrderService.Domain.Seedwork;

public interface IAggregateRepository<T, Tid> where T : AggregateRoot<Tid>
{
    Task AddAsync(T aggregateRoot, CancellationToken cancellationToken);   
}