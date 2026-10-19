namespace myProject02.Services
{
    public interface IPdfService
    {
        Task<byte[]> GenerateVotersPdfAsync();

        Task<byte[]> GenerateUsersPdfAsync();
    }
}
