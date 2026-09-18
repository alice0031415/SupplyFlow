using MediatR;
using Microsoft.Extensions.Logging;
using SupplyFlow.Procurement.Application.DomainEvents;
using SupplyFlow.Procurement.Domain.Needs;

namespace SupplyFlow.Procurement.Application.Needs.Events;

public sealed class TenderPublishedHandler(
    ILogger<TenderPublishedHandler> logger,
    ITenderPublishedPublisher publisher)
    : INotificationHandler<
        DomainEventNotification<TenderPublishedDomainEvent>>
{
    public async Task Handle(
        DomainEventNotification<TenderPublishedDomainEvent> notification,
        CancellationToken cancellationToken)
    {
        var needId = notification.DomainEvent.NeedId;

        logger.LogInformation(
            "Tender published for Need {NeedId}",
            needId);

        await publisher.PublishAsync(
            needId,
            cancellationToken);
    }
}