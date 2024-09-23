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
            var facilityDTOs = await _context.Facilities
                .FromSqlRaw("EXEC GetFacilities")
                .Select(f => new FacilityDTO
                {
                    FacilityID = f.FacilityID,
                    FacilityName = f.FacilityName,
                    Description = f.Description,
                    IsAvailable = f.IsAvailable
                })
                .ToListAsync();

            return Ok(facilityDTOs);
        }


        [HttpGet("{id}")]
        
        public async Task<ActionResult<FacilityDTO>> GetFacility(int id)
        {
            var facility = await _context.Facilities
                .FromSqlRaw("EXEC GetFacilityById @p0", id)
                .FirstOrDefaultAsync();

            if (facility == null)
            {
                return NotFound();
            }

            var facilityDTO = new FacilityDTO
            {
                FacilityID = facility.FacilityID,
                FacilityName = facility.FacilityName,
                Description = facility.Description,
                IsAvailable = facility.IsAvailable
            };

            return Ok(facilityDTO);
        }

        [HttpPost]
        public async Task<ActionResult<FacilityDTO>> CreateFacility(FacilityDTO facilityDTO)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC CreateFacility @p0, @p1, @p2",
                facilityDTO.FacilityName, facilityDTO.Description, facilityDTO.IsAvailable);

            // Optionally, fetch the new facility if needed
            return CreatedAtAction(nameof(GetFacility), new { id = facilityDTO.FacilityID }, facilityDTO);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFacility(int id, FacilityDTO facilityDTO)
        {
            if (id != facilityDTO.FacilityID)
            {
                return BadRequest();
            }

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC UpdateFacility @p0, @p1, @p2, @p3",
                id, facilityDTO.FacilityName, facilityDTO.Description, facilityDTO.IsAvailable);

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFacility(int id)
        {
            var result = await _context.Database.ExecuteSqlRawAsync("EXEC DeleteFacility @p0", id);

            if (result == 0)
            {
                return NotFound();
            }

            return NoContent();
        }


    }
}
