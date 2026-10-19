using myProject02.Dto;

namespace myProject02.Services
{
    public interface Ivoterservice
    {
        Task<IEnumerable<VoterDTO>> GetAllVotersAsync();
        Task<VoterDTO?> GetVoterByIdAsync(int id);
        Task<VoterDTO> CreateVoterAsync(CreateVoterDTO createVoterDto);
    }
}
