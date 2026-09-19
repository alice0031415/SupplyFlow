using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SupplyFlow.Contracts.Events;
using SupplyFlow.Supplier.Domain.Tenders;
using SupplyFlow.Supplier.Infrastructure.Persistence;

namespace SupplyFlow.Supplier.Infrastructure.Messaging.Consumers;

public sealed class TenderPublishedConsumer(
    SupplyFlowSupplierDbContext dbContext,
    ILogger<TenderPublishedConsumer> logger)
    : IConsumer<TenderPublishedIntegrationEvent>
{
    public async Task Consume(
        ConsumeContext<TenderPublishedIntegrationEvent> context)
    {
        var message = context.Message;

        var alreadyReceived = await dbContext.ReceivedTenders
            .AnyAsync(
                x => x.NeedId == message.NeedId,
                context.CancellationToken);

        if (alreadyReceived)
        {
            logger.LogWarning(
                "Tender already processed. NeedId={NeedId}",
                message.NeedId);

            return;
        }

        var receivedTender = new ReceivedTender(
            message.NeedId,
            context.MessageId ?? Guid.Empty,
            context.CorrelationId ?? message.CorrelationId);

        await dbContext.ReceivedTenders.AddAsync(
            receivedTender,
            context.CancellationToken);

        await dbContext.SaveChangesAsync(
            context.CancellationToken);

        logger.LogInformation(
            "Tender received and stored. NeedId={NeedId}, MessageId={MessageId}",
            message.NeedId,
            context.MessageId);
    }
}