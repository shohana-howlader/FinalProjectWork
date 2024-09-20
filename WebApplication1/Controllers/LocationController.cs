using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.DTO;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly TravelDBContext _context;

        public LocationController(TravelDBContext context)
        {
            _context = context;
        }

        private LocationDTO MapToLocationDTO(Location location)
        {
            return new LocationDTO
            {
                LocationID = location.LocationID,
                LocationName = location.LocationName
            };
        }

        private Location MapToLocationEntity(LocationDTO locationDTO)
        {
            return new Location
            {
                LocationID = locationDTO.LocationID,
                LocationName = locationDTO.LocationName
            };
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocationDTO>>> GetLocations()
        {
            var locations = await _context.Locations.ToListAsync();
            var locationDTOs = locations.Select(l => MapToLocationDTO(l)).ToList();
            return locationDTOs;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LocationDTO>> GetLocation(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null) return NotFound();
            return MapToLocationDTO(location);
        }

        [HttpPost]
        public async Task<ActionResult<LocationDTO>> CreateLocation(LocationDTO locationDTO)
        {
            var location = MapToLocationEntity(locationDTO);
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetLocation), new { id = location.LocationID }, MapToLocationDTO(location));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLocation(int id, LocationDTO locationDTO)
        {
            if (id != locationDTO.LocationID) return BadRequest();
            var location = await _context.Locations.FindAsync(id);
            if (location == null) return NotFound();

            location.LocationName = locationDTO.LocationName;

            _context.Entry(location).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null) return NotFound();
            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
