namespace SupplyFlow.Procurement.Application.Logistics;

public interface ILogisticsGateway
{
    Task<ShipmentStatus> GetShipmentStatusAsync(
        string shipmentId,
        int delayMs,
        bool fail,
        CancellationToken cancellationToken);
}

public sealed record ShipmentStatus(
    string ShipmentId,
    string Status,
    DateTime UpdatedAtUtc);