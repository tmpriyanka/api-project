

using Microsoft.EntityFrameworkCore;
using myProject02.Dto;
using myProject02.Models;

namespace myProject02.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly AppDbContext _context;

        public CandidateService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CandidateDTO>> GetAllCandidatesAsync()
        {
            return await _context.Candidates
                .AsNoTracking()
                .Select(c => new CandidateDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Nic = c.Nic,
                    RoleId = c.PartyId,
                    RoleName = c.Party != null ? c.Party.Name : string.Empty
                })
                .ToListAsync();
        }

        public async Task<CandidateDTO?> GetCandidateByIdAsync(int id)
        {
            return await _context.Candidates
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new CandidateDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Nic = c.Nic,
                    RoleId = c.PartyId,
                    RoleName = c.Party != null ? c.Party.Name : string.Empty
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CandidateDTO>> GetCandidatesByPartyIdAsync(int partyId)
        {
            return await _context.Candidates
                .AsNoTracking()
                .Where(c => c.PartyId == partyId)
                .Select(c => new CandidateDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Nic = c.Nic,
                    RoleId = c.PartyId,
                    RoleName = c.Party != null ? c.Party.Name : string.Empty
                })
                .ToListAsync();
        }

        //public async Task<CandidateDTO> CreateCandidateAsync(CreateCandidateDTO candidateDto)
        //{
        //    var candidate = new Candidate
        //    {
        //        Name = candidateDto.Name,
        //        Nic = candidateDto.Nic,
        //        PartyId = candidateDto.RoleId
        //    };

        //    _context.Candidates.Add(candidate);
        //    await _context.SaveChangesAsync();

        //    candidateDto.Id = candidate.Id;
        //    return candidateDto;
        //}

        public async Task<CandidateDTO> CreateCandidateAsync(CreateCandidateDTO candidateDto)
        {
            // Check whether the role exists
            var partyExists = await _context.Parties
                .AnyAsync(r => r.Id == candidateDto.PartyId);

            if (!partyExists)
            {
                throw new ArgumentException("The specified party does not exist.");
            }

            var candidate = new Candidate
            {
                Name = candidateDto.Name,
                PartyId = candidateDto.PartyId
            };

            _context.Candidates.Add(candidate);
            await _context.SaveChangesAsync();

            return await GetCandidateByIdAsync(candidate.Id)
                   ?? throw new InvalidOperationException("Failed to retrieve the created candidate.");
        }

        public async Task<bool> UpdateCandidateAsync(int id, UpdateCandidateDTO candidateDto)
        {
            var candidate = await _context.Candidates.FindAsync(id);
            if (candidate == null)
            {
                return false;
            }

            candidate.Name = candidateDto.Name;
            candidate.Nic = candidateDto.Nic;
            candidate.PartyId = candidateDto.PartyId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCandidateAsync(int id)
        {
            var candidate = await _context.Candidates.FindAsync(id);
            if (candidate == null)
            {
                return false;
            }

            _context.Candidates.Remove(candidate);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
