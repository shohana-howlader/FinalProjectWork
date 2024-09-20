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
            var states = await _context.States.ToListAsync();
            var stateDTOs = states.Select(s => MapToStateDTO(s)).ToList();
            return stateDTOs;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StateDTO>> GetState(int id)
        {
            var state = await _context.States.FindAsync(id);
            if (state == null) return NotFound();
            return MapToStateDTO(state);
        }

        [HttpPost]
        public async Task<ActionResult<StateDTO>> CreateState(StateDTO stateDTO)
        {
            var state = MapToStateEntity(stateDTO);
            _context.States.Add(state);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetState), new { id = state.StateID }, MapToStateDTO(state));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateState(int id, StateDTO stateDTO)
        {
            if (id != stateDTO.StateID) return BadRequest();
            var state = await _context.States.FindAsync(id);
            if (state == null) return NotFound();

            state.StateName = stateDTO.StateName;
            state.CountryID = stateDTO.CountryID;

            _context.Entry(state).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteState(int id)
        {
            var state = await _context.States.FindAsync(id);
            if (state == null) return NotFound();
            _context.States.Remove(state);
            await _context.SaveChangesAsync();
            return NoContent();
        }



    }

}
