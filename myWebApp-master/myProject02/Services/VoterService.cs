using Microsoft.EntityFrameworkCore;
using myProject02.Dto;
using myProject02.Models;
using myProject02.Services;

namespace myProject02.Services
{
    public class VoterService : Ivoterservice
    {
        private readonly AppDbContext _context;

        //private readonly ILogger<VoterService> _logger;

        public VoterService(AppDbContext context)
        {
            _context = context;
        }

        // Mapping helper: Reuse this in Get methods
        private static VoterDTO MapToDto(Voter v) => new VoterDTO
        {
            FullName = v.FullName,
            CreatedAt = v.CreatedAt
        };

        public async Task<IEnumerable<VoterDTO>> GetAllVotersAsync()
        {
            return await _context.Voters
                .Select(v => MapToDto(v))
                .ToListAsync();
        }

        public async Task<VoterDTO?> GetVoterByIdAsync(int id)
        {
            var voter = await _context.Voters.FindAsync(id);
            return voter == null ? null : MapToDto(voter);
        }

        public async Task<VoterDTO> CreateVoterAsync(CreateVoterDTO createVoterDto)
        {
            var voter = new Voter { FullName = createVoterDto.FullName, Email = createVoterDto.Email };

            _context.Voters.Add(voter);
            await _context.SaveChangesAsync();
            return new VoterDTO
            {
                Id = voter.Id,             // This is now available!
                FullName = voter.FullName,
                CreatedAt = voter.CreatedAt
            };
        }

        public async Task<bool> UpdateVoterAsync(int id, VoterDTO voterDto)
        {
            var existingVoter = await _context.Voters.FindAsync(id);
            if (existingVoter == null) return false;

            existingVoter.FullName = voterDto.FullName;

            // DbContext will detect change and set UpdatedAt automatically
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<VoterDTO>> GetPublicVoterDataForTimePeriodAsync(DateTime startDate, DateTime endDate)
        {
            var voters = await _context.Voters
           .Where(v => v.CreatedAt >= startDate && v.CreatedAt <= endDate)
           .ToListAsync();

            return voters.Select(MapToDto);
        }

        public async Task UpdateVoterPessimisticAsync(int voterId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            var voter = await _context.Voters
                .FromSqlInterpolated($"SELECT * FROM Voters WITH (UPDLOCK, ROWLOCK) WHERE Id = {voterId}")
                .FirstOrDefaultAsync();

            if (voter != null)
            {
                voter.FullName = "Updated Name";
                await _context.SaveChangesAsync();
            }

            await transaction.CommitAsync(); // The lock is released here
        }


    }
}
