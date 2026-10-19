

using Microsoft.AspNetCore.Mvc;
using myProject02.Dto;
using myProject02.Services;

namespace myProject02.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartyController : ControllerBase
    {
        private readonly IPartyService _partyService;

        public PartyController(IPartyService partyService)
        {
            _partyService = partyService;
        }

        // GET: api/Party
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PartyDTO>>> GetAllParties()
        {
            var parties = await _partyService.GetAllPartiesAsync();
            return Ok(parties);
        }

        // GET: api/Party/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PartyDTO>> GetPartyById(int id)
        {
            var party = await _partyService.GetPartyByIdAsync(id);
            if (party == null)
            {
                return NotFound($"Party with ID {id} not found.");
            }

            return Ok(party);
        }

        // POST: api/Party
        [HttpPost]
        public async Task<ActionResult<PartyDTO>> CreateParty([FromBody] CreatePartyDTO partyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdParty = await _partyService.CreatePartyAsync(partyDto);
            return CreatedAtAction(nameof(GetPartyById), new { id = createdParty.Id }, createdParty);
        }

        // PUT: api/Party/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateParty(int id, [FromBody] UpdatePartyDTO partyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _partyService.UpdatePartyAsync(id, partyDto);
            if (!result)
            {
                return NotFound($"Party with ID {id} not found.");
            }

            return NoContent();
        }

        // DELETE: api/Party/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteParty(int id)
        {
            var result = await _partyService.DeletePartyAsync(id);
            if (!result)
            {
                return NotFound($"Party with ID {id} not found.");
            }

            return NoContent();
        }
    }
}
