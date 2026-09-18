namespace SupplyFlow.Contracts.Events;

public sealed record TenderPublishedIntegrationEvent
{
    public Guid NeedId { get; init; }

    public DateTime PublishedAtUtc { get; init; }

    public Guid CorrelationId { get; init; }
}