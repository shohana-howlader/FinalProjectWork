using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using WebApplication1.Models;
using WebApplication1.Models.DTO;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly TravelDBContext _context;

        public StateController(TravelDBContext context)
        {
            _context = context;
        }

        private StateDTO MapToStateDTO(State state)
        {
            return new StateDTO
            {
                StateID = state.StateID,
                StateName = state.StateName,
                CountryID = state.CountryID
            };
        }

        private State MapToStateEntity(StateDTO stateDTO)
        {
            return new State
            {
                StateID = stateDTO.StateID,
                StateName = stateDTO.StateName,
                CountryID = stateDTO.CountryID
            };
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StateDTO>>> GetStates()
        {
            var stateDTOs = await _context.States
                .FromSqlRaw("EXEC GetStates")
                .Select(s => new StateDTO
                {
                    StateID = s.StateID,
                    StateName = s.StateName,
                    CountryID = s.CountryID
                })
                .ToListAsync();

            return Ok(stateDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StateDTO>> GetState(int id)
        {
            var state = await _context.States
                .FromSqlRaw("EXEC GetStateById @p0", id)
                .FirstOrDefaultAsync();

            if (state == null)
            {
                return NotFound();
            }

            var stateDTO = new StateDTO
            {
                StateID = state.StateID,
                StateName = state.StateName,
                CountryID = state.CountryID
            };

            return Ok(stateDTO);
        }


        [HttpPost]
        public async Task<ActionResult<StateDTO>> CreateState(StateDTO stateDTO)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC CreateState @p0, @p1",
                stateDTO.StateName, stateDTO.CountryID);

            // Optionally, fetch the new state if needed
            return CreatedAtAction(nameof(GetState), new { id = stateDTO.StateID }, stateDTO);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateState(int id, StateDTO stateDTO)
        {
            if (id != stateDTO.StateID)
            {
                return BadRequest();
            }

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC UpdateState @p0, @p1, @p2",
                id, stateDTO.StateName, stateDTO.CountryID);

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteState(int id)
        {
            var result = await _context.Database.ExecuteSqlRawAsync("EXEC DeleteState @p0", id);

            if (result == 0)
            {
                return NotFound();
            }

            return NoContent();
        }




    }

}
