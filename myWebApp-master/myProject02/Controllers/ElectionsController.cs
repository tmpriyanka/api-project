using Microsoft.AspNetCore.Mvc;
using myProject02.Dto;
using myProject02.Services;

namespace myProject02.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ElectionsController : ControllerBase
    {
        private readonly IElectionService _electionService;

        public ElectionsController(IElectionService electionService)
        {
            _electionService = electionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ElectionDto>>> GetAll()
        {
            var elections = await _electionService.GetAllElectionsAsync();
            return Ok(elections);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ElectionDetailDto>> GetById(int id)
        {
            var election = await _electionService.GetElectionByIdAsync(id);
            if (election == null) return NotFound();
            return Ok(election);
        }

        [HttpPost]
        public async Task<ActionResult<ElectionDto>> Create([FromBody] CreateElectionDto dto)
        {
            var election = await _electionService.CreateElectionAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = election.Id }, election);
        }

        [HttpPost("{electionId}/parties/{partyId}")]
        public async Task<IActionResult> AddPartyToElection(int electionId, int partyId)
        {
            var result = await _electionService.AddPartyToElectionAsync(electionId, partyId);
            if (!result) return BadRequest("Invalid election or party ID.");
            return Ok();
        }

        [HttpDelete("{electionId}/parties/{partyId}")]
        public async Task<IActionResult> RemovePartyFromElection(int electionId, int partyId)
        {
            var result = await _electionService.RemovePartyFromElectionAsync(electionId, partyId);
            if (!result) return NotFound("Relationship does not exist.");
            return NoContent();
        }
    }
}

