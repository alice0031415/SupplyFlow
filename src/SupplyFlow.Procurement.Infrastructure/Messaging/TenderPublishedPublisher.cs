using MassTransit;
using SupplyFlow.Contracts.Events;
using SupplyFlow.Procurement.Application.Needs.Events;

namespace SupplyFlow.Procurement.Infrastructure.Messaging;

public sealed class TenderPublishedPublisher(
    IPublishEndpoint publishEndpoint)
    : ITenderPublishedPublisher
{
    public Task PublishAsync(
        Guid needId,
        CancellationToken cancellationToken)
    {
        var message = new TenderPublishedIntegrationEvent
        {
            NeedId = needId,
            PublishedAtUtc = DateTime.UtcNow,
            CorrelationId = needId
        };

        return publishEndpoint.Publish(
            message,
            context => context.CorrelationId = needId,
            cancellationToken);
    }
}