
using Microsoft.EntityFrameworkCore;
using myProject02.Dto;
using myProject02.Models;
using System.Data;

namespace myProject02.Services
{
    public class PartyService : IPartyService
    {
        private readonly AppDbContext _context;

        public PartyService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PartyDTO>> GetAllPartiesAsync()
        {
            return await _context.Parties
                .AsNoTracking()
                .Select(p => new PartyDTO
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToListAsync();
        }

        public async Task<PartyDTO?> GetPartyByIdAsync(int id)
        {
            return await _context.Parties
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new PartyDTO
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .FirstOrDefaultAsync();
        }

        public async Task<PartyDTO> CreatePartyAsync(CreatePartyDTO partyDto)
        {
            var party = new Party
            {
                Name = partyDto.Name
            };

            _context.Parties.Add(party);
            await _context.SaveChangesAsync();

            return new PartyDTO
            {
                Id = party.Id,
                Name = party.Name
            };
        }

        public async Task<bool> UpdatePartyAsync(int id, UpdatePartyDTO partyDto)
        {
            var party = await _context.Parties.FindAsync(id);
            if (party == null)
            {
                return false;
            }

            party.Name = partyDto.Name;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePartyAsync(int id)
        {
            var party = await _context.Parties.FindAsync(id);
            if (party == null)
            {
                return false;
            }

            _context.Parties.Remove(party);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
