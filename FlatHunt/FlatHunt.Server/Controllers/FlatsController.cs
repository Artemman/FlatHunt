using FlatHunt.Server.Auth;
using FlatHunt.Server.Dto;
using FlatHunt.Server.Services.Flats.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlatHunt.Server.Controllers
{
    [ApiController]
    [Route("api/flats")]
    [Authorize(Roles = $"{Roles.Broker},{Roles.Admin}")]
    public class FlatsController : ControllerBase
    {
        private readonly IFlatsService _service;

        public FlatsController(IFlatsService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] FlatFilterRequest filter, CancellationToken ct)
        {
            var result = await _service.GetFlatsAsync(filter, ct);
            return Ok(result);
        }

    }
}