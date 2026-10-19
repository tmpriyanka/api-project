using Microsoft.EntityFrameworkCore;
using myProject02.Dto;
using myProject02.Models;

namespace myProject02.Services
{
    public class ElectionService : IElectionService
    {
        private readonly AppDbContext _context;

        public ElectionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ElectionDto>> GetAllElectionsAsync()
        {
            return await _context.Elections
                .Select(e => new ElectionDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Year = e.Year
                })
                .ToListAsync();
        }

        public async Task<ElectionDetailDto?> GetElectionByIdAsync(int id)
        {
            var election = await _context.Elections
                .Include(e => e.PartyElections)
                    .ThenInclude(pe => pe.Party)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (election == null) return null;

            return new ElectionDetailDto
            {
                Id = election.Id,
                Name = election.Name,
                Year = election.Year,
                Parties = election.PartyElections.Select(pe => new PartyDTO
                {
                    Id = pe.Party.Id,
                    Name = pe.Party.Name,
                }).ToList()
            };
        }

        public async Task<ElectionDto> CreateElectionAsync(CreateElectionDto electionDto)
        {
            var election = new Election
            {
                Name = electionDto.Name,
                Year = electionDto.Year
            };

            _context.Elections.Add(election);
            await _context.SaveChangesAsync();

            return new ElectionDto
            {
                Id = election.Id,
                Name = election.Name,
                Year = election.Year
            };
        }

        public async Task<bool> AddPartyToElectionAsync(int electionId, int partyId)
        {
            var exists = await _context.PartyElections
                .AnyAsync(pe => pe.ElectionId == electionId && pe.PartyId == partyId);

            if (exists) return true; // Already linked

            var partyExists = await _context.Parties.AnyAsync(p => p.Id == partyId);
            var electionExists = await _context.Elections.AnyAsync(e => e.Id == electionId);

            if (!partyExists || !electionExists) return false;

            _context.PartyElections.Add(new PartyElection
            {
                ElectionId = electionId,
                PartyId = partyId
            });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemovePartyFromElectionAsync(int electionId, int partyId)
        {
            var partyElection = await _context.PartyElections
                .FirstOrDefaultAsync(pe => pe.ElectionId == electionId && pe.PartyId == partyId);

            if (partyElection == null) return false;

            _context.PartyElections.Remove(partyElection);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}