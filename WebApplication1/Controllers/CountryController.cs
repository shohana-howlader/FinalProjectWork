using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.DTO;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly TravelDBContext _context;

        public CountryController(TravelDBContext context)
        {
            _context = context;
        }

        // Helper method to manually map Country entity to CountryDTO
        private CountryDTO MapToCountryDTO(Country country)
        {
            return new CountryDTO
            {
                CountryID = country.CountryID,
                CountryName = country.CountryName
            };
        }

        // Helper method to manually map CountryDTO to Country entity
        private Country MapToCountryEntity(CountryDTO countryDTO)
        {
            return new Country
            {
                CountryID = countryDTO.CountryID,
                CountryName = countryDTO.CountryName
            };
        }

        // GET: api/country
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryDTO>>> GetCountries()
        {
            var countries = await _context.Countries.ToListAsync();
            var countryDTOs = countries.Select(c => MapToCountryDTO(c)).ToList();
            return countryDTOs;
        }

        // GET: api/country/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDTO>> GetCountry(int id)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country == null) return NotFound();
            return MapToCountryDTO(country);
        }

        // POST: api/country
        [HttpPost]
        public async Task<ActionResult<CountryDTO>> CreateCountry(CountryDTO countryDTO)
        {
            var country = MapToCountryEntity(countryDTO);
            _context.Countries.Add(country);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCountry), new { id = country.CountryID }, MapToCountryDTO(country));
        }

        // PUT: api/country/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCountry(int id, CountryDTO countryDTO)
        {
            if (id != countryDTO.CountryID) return BadRequest();

            var country = await _context.Countries.FindAsync(id);
            if (country == null) return NotFound();

            // Update the fields manually
            country.CountryName = countryDTO.CountryName;

            _context.Entry(country).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/country/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country == null) return NotFound();
            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
