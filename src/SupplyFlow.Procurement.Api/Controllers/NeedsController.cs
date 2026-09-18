using MediatR;
using Microsoft.AspNetCore.Mvc;
using SupplyFlow.Procurement.Application.Needs;
using SupplyFlow.Procurement.Application.Needs.Commands;
using SupplyFlow.Procurement.Application.Needs.Queries;

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

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<NeedDto>>> GetAll(
    CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new GetNeedsQuery(),
            cancellationToken);

        return Ok(result);
    }

    [HttpPost("{id:guid}/publish")]
    public async Task<IActionResult> Publish(
    Guid id,
    CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new PublishTenderCommand(id),
            cancellationToken);

        return result
            ? NoContent()
            : NotFound();
    }
}