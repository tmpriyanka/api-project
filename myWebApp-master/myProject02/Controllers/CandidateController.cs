

using Microsoft.AspNetCore.Mvc;
using myProject02.Dto;
using myProject02.Services;

namespace myProject02.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandidateController : ControllerBase
    {
        private readonly ICandidateService _candidateService;

        public CandidateController(ICandidateService candidateService)
        {
            _candidateService = candidateService;
        }

        // GET: api/Candidate
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CandidateDTO>>> GetAllCandidates()
        {
            var candidates = await _candidateService.GetAllCandidatesAsync();
            return Ok(candidates);
        }

        // GET: api/Candidate/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CandidateDTO>> GetCandidateById(int id)
        {
            var candidate = await _candidateService.GetCandidateByIdAsync(id);
            if (candidate == null)
            {
                return NotFound($"Candidate with ID {id} not found.");
            }

            return Ok(candidate);
        }

        // GET: api/Candidate/party/2
        [HttpGet("party/{partyId:int}")]
        public async Task<ActionResult<IEnumerable<CandidateDTO>>> GetCandidatesByPartyId(int partyId)
        {
            var candidates = await _candidateService.GetCandidatesByPartyIdAsync(partyId);
            return Ok(candidates);
        }

        // POST: api/Candidate
        [HttpPost]
        public async Task<ActionResult<CandidateDTO>> CreateCandidate([FromBody] CreateCandidateDTO candidateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdCandidate = await _candidateService.CreateCandidateAsync(candidateDto);
            return CreatedAtAction(nameof(GetCandidateById), new { id = createdCandidate.Id }, createdCandidate);
        }

        // PUT: api/Candidate/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCandidate(int id, [FromBody] UpdateCandidateDTO candidateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _candidateService.UpdateCandidateAsync(id, candidateDto);
            if (!result)
            {
                return NotFound($"Candidate with ID {id} not found.");
            }

            return NoContent();
        }

        // DELETE: api/Candidate/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCandidate(int id)
        {
            var result = await _candidateService.DeleteCandidateAsync(id);
            if (!result)
            {
                return NotFound($"Candidate with ID {id} not found.");
            }

            return NoContent();
        }
    }
}
