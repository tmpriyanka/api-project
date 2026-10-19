using myProject02.Dto;
using myProject02.Models;
using Microsoft.EntityFrameworkCore;

namespace myProject02.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    RoleId = u.RoleId,
                    RoleName = u.Role.Name
                })
                .ToListAsync();
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Where(u => u.Id == id)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    RoleId = u.RoleId,
                    RoleName = u.Role.Name
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserDto>> GetUsersByRoleAsync(int roleId)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Where(u => u.RoleId == roleId)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    RoleId = u.RoleId,
                    RoleName = u.Role.Name
                })
                .ToListAsync();
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
        {
            // Check whether the role exists
            var roleExists = await _context.Roles
                .AnyAsync(r => r.Id == dto.RoleId);

            if (!roleExists)
            {
                throw new ArgumentException("The specified role does not exist.");
            }

            var user = new User
            {
                UserName = dto.UserName,
                RoleId = dto.RoleId
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return await GetUserByIdAsync(user.Id)
                   ?? throw new InvalidOperationException("Failed to retrieve the created user.");
        }

        public async Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return null;
            }

            // Check whether the new role exists
            var roleExists = await _context.Roles
                .AnyAsync(r => r.Id == dto.RoleId);

            if (!roleExists)
            {
                throw new ArgumentException("The specified role does not exist.");
            }

            user.UserName = dto.UserName;
            user.RoleId = dto.RoleId;

            await _context.SaveChangesAsync();

            return await GetUserByIdAsync(user.Id);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
