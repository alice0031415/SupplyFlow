using Refit;

namespace SupplyFlow.Procurement.Infrastructure.Logistics;

public interface ILogisticsApi
{
    [Get("/external/logistics/shipments/{shipmentId}")]
    Task<ShipmentStatusResponse> GetShipmentStatusAsync(
        string shipmentId,
        int delayMs = 0,
        bool fail = false,
        CancellationToken cancellationToken = default);
}

public sealed record ShipmentStatusResponse(
    string ShipmentId,
    string Status,
    DateTime UpdatedAtUtc);