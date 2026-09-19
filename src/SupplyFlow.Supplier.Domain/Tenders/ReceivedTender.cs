namespace SupplyFlow.Supplier.Domain.Tenders;

public sealed class ReceivedTender
{
    private ReceivedTender()
    {
    }

    public ReceivedTender(
        Guid needId,
        Guid messageId,
        Guid correlationId)
    {
        Id = Guid.NewGuid();
        NeedId = needId;
        MessageId = messageId;
        CorrelationId = correlationId;
        ReceivedAtUtc = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public Guid NeedId { get; private set; }

    public Guid MessageId { get; private set; }

    public Guid CorrelationId { get; private set; }

    public DateTime ReceivedAtUtc { get; private set; }
}