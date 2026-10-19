using myProject02.Dto;

namespace myProject02.Services
{
    public interface IPartyService
    {
        Task<IEnumerable<PartyDTO>> GetAllPartiesAsync();
        Task<PartyDTO?> GetPartyByIdAsync(int id);
        Task<PartyDTO> CreatePartyAsync(CreatePartyDTO createPartyDto);
        Task<bool> UpdatePartyAsync(int id, UpdatePartyDTO updatePartyDto);
        Task<bool> DeletePartyAsync(int id);
    }
}
