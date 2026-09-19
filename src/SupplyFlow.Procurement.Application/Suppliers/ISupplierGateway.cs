namespace SupplyFlow.Procurement.Application.Suppliers;

public interface ISupplierGateway
{
    Task<SupplierTenderStatus> GetTenderStatusAsync(
        Guid needId,
        CancellationToken cancellationToken);
}

public sealed record SupplierTenderStatus(
    Guid NeedId,
    bool Received,
    DateTimeOffset? ReceivedAtUtc);