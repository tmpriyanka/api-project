using myProject02.Dto;

namespace myProject02.Services
{
    public interface IRoleService
    {
        //Task<IEnumerable<RoleDTO>> GetAllRolesAsync();
        Task<RoleDTO?> GetRoleByIdAsync(int id);
        Task<RoleDTO?> CreateRoleAsync(CreateRoleDTO createRoleDto);
    }
}
