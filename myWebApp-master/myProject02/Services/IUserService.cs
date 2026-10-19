using myProject02.Dto;

namespace myProject02.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();

        Task<UserDto?> GetUserByIdAsync(int id);

        Task<IEnumerable<UserDto>> GetUsersByRoleAsync(int roleId);

        Task<UserDto> CreateUserAsync(CreateUserDto dto);

        Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto dto);

        Task<bool> DeleteUserAsync(int id);
    }
}
