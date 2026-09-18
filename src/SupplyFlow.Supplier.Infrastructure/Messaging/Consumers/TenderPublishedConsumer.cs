using MassTransit;
using Microsoft.Extensions.Logging;
using SupplyFlow.Contracts.Events;

namespace SupplyFlow.Supplier.Infrastructure.Messaging.Consumers;

public sealed class TenderPublishedConsumer(
    ILogger<TenderPublishedConsumer> logger)
    : IConsumer<TenderPublishedIntegrationEvent>
{
    public Task Consume(
        ConsumeContext<TenderPublishedIntegrationEvent> context)
    {
        var message = context.Message;

        logger.LogInformation(
            "Received TenderPublished. NeedId={NeedId}, CorrelationId={CorrelationId}",
            message.NeedId,
            context.CorrelationId);

        return Task.CompletedTask;
    }
}