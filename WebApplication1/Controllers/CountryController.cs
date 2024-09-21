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
            var countries = await _context.Countries
                .FromSqlRaw("EXEC GetCountries")
                .ToListAsync();

            var countryDTOs = countries.Select(c => MapToCountryDTO(c)).ToList();
            return countryDTOs;
        }


        // GET: api/country/{id}
        
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDTO>> GetCountry(int id)
        {
            var country = await _context.Countries
                .FromSqlRaw("EXEC GetCountryById @CountryID = {0}", id)
                .FirstOrDefaultAsync();

            if (country == null) return NotFound();
            return MapToCountryDTO(country);
        }


        // POST: api/country
        [HttpPost]
        public async Task<ActionResult<CountryDTO>> CreateCountry(CountryDTO countryDTO)
        {
            var countryName = countryDTO.CountryName;

            var newCountryID = await _context.Database
                .ExecuteSqlRawAsync("EXEC InsertCountry @CountryName = {0}", countryName);

            return CreatedAtAction(nameof(GetCountry), new { id = newCountryID }, countryDTO);
        }


        // PUT: api/country/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCountry(int id, CountryDTO countryDTO)
        {
            if (id != countryDTO.CountryID) return BadRequest();

            var result = await _context.Database
                .ExecuteSqlRawAsync("EXEC UpdateCountry @CountryID = {0}, @CountryName = {1}",
                    id, countryDTO.CountryName);

            if (result == 0) return NotFound();

            return NoContent();
        }


        // DELETE: api/country/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var result = await _context.Database
                .ExecuteSqlRawAsync("EXEC DeleteCountry @CountryID = {0}", id);

            if (result == 0) return NotFound();

            return NoContent();
        }

    }
}
