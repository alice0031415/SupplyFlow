namespace SupplyFlow.Procurement.Application.Needs.Events;

public interface ITenderPublishedPublisher
{
    Task PublishAsync(
        Guid needId,
        CancellationToken cancellationToken);
}