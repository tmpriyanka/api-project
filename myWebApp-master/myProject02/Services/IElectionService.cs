using myProject02.Dto;

namespace myProject02.Services
{
    public interface IElectionService
    {
        Task<IEnumerable<ElectionDto>> GetAllElectionsAsync();
        Task<ElectionDetailDto?> GetElectionByIdAsync(int id);
        Task<ElectionDto> CreateElectionAsync(CreateElectionDto electionDto);
        Task<bool> AddPartyToElectionAsync(int electionId, int partyId);
        Task<bool> RemovePartyFromElectionAsync(int electionId, int partyId);
    }
}
