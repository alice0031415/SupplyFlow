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
            roles = User.Claims
                .Where(x => x.Type == "roles")
                .Select(x => x.Value)
        });
    }

    [Authorize(Roles = "procurement")]
    [HttpGet("procurement")]
    public IActionResult GetProcurement()
    {
        return Ok(new
        {
            message = "Procurement role accepted."
        });
    }
}