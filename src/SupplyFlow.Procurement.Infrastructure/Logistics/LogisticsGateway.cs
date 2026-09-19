using SupplyFlow.Procurement.Application.Logistics;

namespace SupplyFlow.Procurement.Infrastructure.Logistics;

public sealed class LogisticsGateway(
    ILogisticsApi client)
    : ILogisticsGateway
{
    public async Task<ShipmentStatus> GetShipmentStatusAsync(
        string shipmentId,
        int delayMs,
        bool fail,
        CancellationToken cancellationToken)
    {
        var response = await client.GetShipmentStatusAsync(
            shipmentId,
            delayMs,
            fail,
            cancellationToken);

        return new ShipmentStatus(
            response.ShipmentId,
            response.Status,
            response.UpdatedAtUtc);
    }
}