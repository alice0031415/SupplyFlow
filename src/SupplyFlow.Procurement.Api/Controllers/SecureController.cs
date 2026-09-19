using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SupplyFlow.Procurement.Api.Controllers;

[ApiController]
[Route("api/secure")]
public sealed class SecureController : ControllerBase
{
    [Authorize]
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            message = "Authenticated",
            user = User.Identity?.Name,
            claims = User.Claims.Select(x => new
            {
                x.Type,
                x.Value
            })
        });
    }
}