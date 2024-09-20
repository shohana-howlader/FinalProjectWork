using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.DTO;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageFacilityController : ControllerBase
    {
        private readonly TravelDBContext _context;

        public PackageFacilityController(TravelDBContext context)
        {
            _context = context;
        }

        private PackageFacilityDTO MapToPackageFacilityDTO(PackageFacility packageFacility)
        {
            return new PackageFacilityDTO
            {
                PackageFacilityID = packageFacility.PackageFacilityID,
                PackageID = packageFacility.PackageID,
                FacilityID = packageFacility.FacilityID
            };
        }

        private PackageFacility MapToPackageFacilityEntity(PackageFacilityDTO packageFacilityDTO)
        {
            return new PackageFacility
            {
                PackageFacilityID = packageFacilityDTO.PackageFacilityID,
                PackageID = packageFacilityDTO.PackageID,
                FacilityID = packageFacilityDTO.FacilityID
            };
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PackageFacilityDTO>>> GetPackageFacilities()
        {
            var packageFacilities = await _context.PackageFacilities.ToListAsync();
            var packageFacilityDTOs = packageFacilities.Select(pf => MapToPackageFacilityDTO(pf)).ToList();
            return packageFacilityDTOs;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PackageFacilityDTO>> GetPackageFacility(int id)
        {
            var packageFacility = await _context.PackageFacilities.FindAsync(id);
            if (packageFacility == null) return NotFound();
            return MapToPackageFacilityDTO(packageFacility);
        }

        [HttpPost]
        public async Task<ActionResult<PackageFacilityDTO>> CreatePackageFacility(PackageFacilityDTO packageFacilityDTO)
        {
            var packageFacility = MapToPackageFacilityEntity(packageFacilityDTO);
            _context.PackageFacilities.Add(packageFacility);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPackageFacility), new { id = packageFacility.PackageFacilityID }, MapToPackageFacilityDTO(packageFacility));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePackageFacility(int id, PackageFacilityDTO packageFacilityDTO)
        {
            if (id != packageFacilityDTO.PackageFacilityID) return BadRequest();
            var packageFacility = await _context.PackageFacilities.FindAsync(id);
            if (packageFacility == null) return NotFound();

            packageFacility.PackageID = packageFacilityDTO.PackageID;
            packageFacility.FacilityID = packageFacilityDTO.FacilityID;

            _context.Entry(packageFacility).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackageFacility(int id)
        {
            var packageFacility = await _context.PackageFacilities.FindAsync(id);
            if (packageFacility == null) return NotFound();
            _context.PackageFacilities.Remove(packageFacility);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
