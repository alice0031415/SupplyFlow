using Microsoft.AspNetCore.Mvc;

namespace SupplyFlow.Logistics.Api.Controllers;

[ApiController]
[Route("external/logistics")]
public sealed class ExternalLogisticsController : ControllerBase
{
    [HttpGet("shipments/{shipmentId}")]
    public async Task<IActionResult> GetShipmentStatus(
        string shipmentId,
        [FromQuery] int delayMs = 0,
        [FromQuery] bool fail = false,
        CancellationToken cancellationToken = default)
    {
        if (delayMs > 0)
        {
            await Task.Delay(delayMs, cancellationToken);
        }

        if (fail)
        {
            return StatusCode(
                StatusCodes.Status503ServiceUnavailable,
                new
                {
                    error = "External logistics service unavailable."
                });
        }

        return Ok(new
        {
            shipmentId,
            status = "InTransit",
            updatedAtUtc = DateTime.UtcNow
        });
    }
}