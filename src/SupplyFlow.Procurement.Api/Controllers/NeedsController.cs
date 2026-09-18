using MediatR;
using Microsoft.AspNetCore.Mvc;
using SupplyFlow.Procurement.Application.Needs.Commands;

namespace SupplyFlow.Procurement.Api.Controllers;

[ApiController]
[Route("api/needs")]
public sealed class NeedsController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateNeedCommand command,
        CancellationToken cancellationToken)
    {
        var id = await sender.Send(command, cancellationToken);

        return Created($"/api/needs/{id}", new { id });
    }
}