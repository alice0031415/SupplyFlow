using Microsoft.AspNetCore.Mvc;
using SupplyFlow.Procurement.Application.Logistics;

namespace SupplyFlow.Procurement.Api.Controllers;

[ApiController]
[Route("api/logistics")]
public sealed class LogisticsController(
    ILogisticsGateway logisticsGateway)
    : ControllerBase
{
    [HttpGet("shipments/{shipmentId}")]
    public async Task<IActionResult> GetShipmentStatus(
        string shipmentId,
        [FromQuery] int delayMs = 0,
        [FromQuery] bool fail = false,
        CancellationToken cancellationToken = default)
    {
        var result = await logisticsGateway.GetShipmentStatusAsync(
            shipmentId,
            delayMs,
            fail,
            cancellationToken);

        return Ok(result);
    }
}