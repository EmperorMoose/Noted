using Microsoft.AspNetCore.Mvc;
using Noted.Application.Data;

namespace Noted.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{

    private readonly NotedDbContext _context;

    public HealthController(NotedDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try 
        {
            await _context.Database.CanConnectAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}