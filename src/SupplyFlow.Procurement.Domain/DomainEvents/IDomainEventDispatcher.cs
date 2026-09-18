using SupplyFlow.Procurement.Domain.Common;

namespace SupplyFlow.Procurement.Application.DomainEvents;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(
        IEnumerable<IDomainEvent> events,
        CancellationToken cancellationToken);
}