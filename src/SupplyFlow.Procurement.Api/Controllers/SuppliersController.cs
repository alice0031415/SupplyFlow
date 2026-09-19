using MediatR;
using Microsoft.AspNetCore.Mvc;
using SupplyFlow.Procurement.Application.Suppliers;

namespace SupplyFlow.Procurement.Api.Controllers;

[ApiController]
[Route("api/suppliers")]
public sealed class SuppliersController(ISender sender)
    : ControllerBase
{
    [HttpGet("tenders/{needId:guid}")]
    public async Task<IActionResult> GetTenderStatus(
        Guid needId,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new GetTenderStatusQuery(needId),
            cancellationToken);

        return Ok(result);
    }
}