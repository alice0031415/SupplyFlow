using MediatR;
using SupplyFlow.Procurement.Application.DomainEvents;
using SupplyFlow.Procurement.Domain.Common;

namespace SupplyFlow.Procurement.Infrastructure.Persistence;

public sealed class MediatRDomainEventDispatcher(
    IPublisher publisher)
    : IDomainEventDispatcher
{
    public async Task DispatchAsync(
        IEnumerable<IDomainEvent> events,
        CancellationToken cancellationToken)
    {
        foreach (var domainEvent in events)
        {
            var notificationType =
                typeof(DomainEventNotification<>)
                    .MakeGenericType(domainEvent.GetType());

            var notification =
                Activator.CreateInstance(
                    notificationType,
                    domainEvent)!;

            await publisher.Publish(
                (INotification)notification,
                cancellationToken);
        }
    }
}