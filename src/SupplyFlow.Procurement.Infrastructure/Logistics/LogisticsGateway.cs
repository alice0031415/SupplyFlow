using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using SupplyFlow.Procurement.Application.Logistics;

namespace SupplyFlow.Procurement.Infrastructure.Logistics;

public sealed class LogisticsGateway(
    ILogisticsApi client,
    IDistributedCache cache)
    : ILogisticsGateway
{
    public async Task<ShipmentStatus> GetShipmentStatusAsync(
        string shipmentId,
        int delayMs,
        bool fail,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"logistics:shipment:{shipmentId}";

        // Test parameters intentionally bypass cache.
        if (!fail && delayMs == 0)
        {
            var cached = await cache.GetStringAsync(
                cacheKey,
                cancellationToken);

            if (cached is not null)
            {
                return JsonSerializer.Deserialize<ShipmentStatus>(
                    cached)!;
            }
        }

        var response = await client.GetShipmentStatusAsync(
            shipmentId,
            delayMs,
            fail,
            cancellationToken);

        var result = new ShipmentStatus(
            response.ShipmentId,
            response.Status,
            response.UpdatedAtUtc);

        // Cache only successful normal requests.
        if (!fail && delayMs == 0)
        {
            await cache.SetStringAsync(
                cacheKey,
                JsonSerializer.Serialize(result),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromSeconds(30)
                },
                cancellationToken);
        }

        return result;
    }
}