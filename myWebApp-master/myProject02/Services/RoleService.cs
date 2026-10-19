using Microsoft.EntityFrameworkCore;
using myProject02.Dto;
using myProject02.Models;
using myProject02.Services;

namespace myProject02.Services
{
    public class RoleService : IRoleService
    {
        private readonly AppDbContext _context;

        public RoleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RoleDTO?> CreateRoleAsync(CreateRoleDTO createRoleDto)
        {
            var roleExists = await _context.Roles
                .AnyAsync(r => r.Name.ToLower() == createRoleDto.Name.Trim().ToLower());

            if (roleExists) return null;
            var role = new Role
            {
                Name = createRoleDto.Name.Trim()
            };

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return new RoleDTO
            {
                Id = role.Id,
                Name = role.Name
            };

        }

        public async Task<RoleDTO?> GetRoleByIdAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return null;

            return new RoleDTO
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        
    }
}
