namespace SupplyFlow.Procurement.Domain.Needs;

public class Need
{
    private Need()
    {
    }

    public Need(
        string description,
        decimal quantity,
        DateOnly requiredBy)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required.", nameof(description));

        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity));

        Id = Guid.NewGuid();
        Description = description;
        Quantity = quantity;
        RequiredBy = requiredBy;
        Status = NeedStatus.Draft;
    }

    public Guid Id { get; private set; }

    public string Description { get; private set; } = null!;

    public decimal Quantity { get; private set; }

    public DateOnly RequiredBy { get; private set; }

    public NeedStatus Status { get; private set; }

    // PostgreSQL xmin — optimistic concurrency token.
    public uint Version { get; private set; }
}