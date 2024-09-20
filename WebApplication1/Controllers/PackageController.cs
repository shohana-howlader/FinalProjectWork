using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.DTO;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly TravelDBContext _context;

        public PackageController(TravelDBContext context)
        {
            _context = context;
        }

        private PackageDTO MapToPackageDTO(Package package)
        {
            return new PackageDTO
            {
                PackageID = package.PackageID,
                PackageTitle = package.PackageTitle
            };
        }

        private Package MapToPackageEntity(PackageDTO packageDTO)
        {
            return new Package
            {
                PackageID = packageDTO.PackageID,
                PackageTitle = packageDTO.PackageTitle
            };
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PackageDTO>>> GetPackages()
        {
            var packages = await _context.Packages.ToListAsync();
            var packageDTOs = packages.Select(p => MapToPackageDTO(p)).ToList();
            return packageDTOs;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PackageDTO>> GetPackage(int id)
        {
            var package = await _context.Packages.FindAsync(id);
            if (package == null) return NotFound();
            return MapToPackageDTO(package);
        }

        [HttpPost]
        public async Task<ActionResult<PackageDTO>> CreatePackage(PackageDTO packageDTO)
        {
            var package = MapToPackageEntity(packageDTO);
            _context.Packages.Add(package);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPackage), new { id = package.PackageID }, MapToPackageDTO(package));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePackage(int id, PackageDTO packageDTO)
        {
            if (id != packageDTO.PackageID) return BadRequest();
            var package = await _context.Packages.FindAsync(id);
            if (package == null) return NotFound();

            package.PackageTitle = packageDTO.PackageTitle;

            _context.Entry(package).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackage(int id)
        {
            var package = await _context.Packages.FindAsync(id);
            if (package == null) return NotFound();
            _context.Packages.Remove(package);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
