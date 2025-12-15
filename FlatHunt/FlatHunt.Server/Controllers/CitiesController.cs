using FlatHunt.Server.Repositories.Interfaces;
using FlatHunt.Server.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlatHunt.Server.Controllers
{
    [ApiController]
    [Route("api/cities")]
    [Authorize]
    public class CitiesController : ControllerBase
    {
        private readonly ICityRepository _cityRepository;

        public CitiesController(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<CityDto>>> GetAll()
        {
            var cities = await _cityRepository.GetAll();
            var dto = cities.Select(c => new CityDto { Id = c.Id, Name = c.Name }).ToList();
            return Ok(dto);
        }
    }
}