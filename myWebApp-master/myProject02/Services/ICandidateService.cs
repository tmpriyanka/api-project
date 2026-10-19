using myProject02.Dto;

namespace myProject02.Services
{
    public interface ICandidateService
    {
        Task<IEnumerable<CandidateDTO>> GetAllCandidatesAsync();
        Task<CandidateDTO?> GetCandidateByIdAsync(int id);
        Task<IEnumerable<CandidateDTO>> GetCandidatesByPartyIdAsync(int partyId);
        Task<CandidateDTO> CreateCandidateAsync(CreateCandidateDTO createCandicateDto);
        Task<bool> UpdateCandidateAsync(int id, UpdateCandidateDTO UpdateCandicateDto);
        Task<bool> DeleteCandidateAsync(int id);
    }
}
