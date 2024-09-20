using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.DTO;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacilityController : ControllerBase
    {
        private readonly TravelDBContext _context;

        public FacilityController(TravelDBContext context)
        {
            _context = context;
        }

        private FacilityDTO MapToFacilityDTO(Facility facility)
        {
            return new FacilityDTO
            {
                FacilityID = facility.FacilityID,
                FacilityName = facility.FacilityName,
                Description = facility.Description,
                IsAvailable = facility.IsAvailable
            };
        }

        private Facility MapToFacilityEntity(FacilityDTO facilityDTO)
        {
            return new Facility
            {
                FacilityID = facilityDTO.FacilityID,
                FacilityName = facilityDTO.FacilityName,
                Description = facilityDTO.Description,
                IsAvailable = facilityDTO.IsAvailable
            };
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FacilityDTO>>> GetFacilities()
        {
            var facilities = await _context.Facilities.ToListAsync();
            var facilityDTOs = facilities.Select(f => MapToFacilityDTO(f)).ToList();
            return facilityDTOs;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FacilityDTO>> GetFacility(int id)
        {
            var facility = await _context.Facilities.FindAsync(id);
            if (facility == null) return NotFound();
            return MapToFacilityDTO(facility);
        }

        [HttpPost]
        public async Task<ActionResult<FacilityDTO>> CreateFacility(FacilityDTO facilityDTO)
        {
            var facility = MapToFacilityEntity(facilityDTO);
            _context.Facilities.Add(facility);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFacility), new { id = facility.FacilityID }, MapToFacilityDTO(facility));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFacility(int id, FacilityDTO facilityDTO)
        {
            if (id != facilityDTO.FacilityID) return BadRequest();
            var facility = await _context.Facilities.FindAsync(id);
            if (facility == null) return NotFound();

            facility.FacilityName = facilityDTO.FacilityName;
            facility.Description = facilityDTO.Description;
            facility.IsAvailable = facilityDTO.IsAvailable;

            _context.Entry(facility).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFacility(int id)
        {
            var facility = await _context.Facilities.FindAsync(id);
            if (facility == null) return NotFound();
            _context.Facilities.Remove(facility);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
