using Microsoft.AspNetCore.Mvc;
using myProject02.Dto;
using myProject02.Services;

namespace myProject02.Controllers
{
    
[ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPdfService _pdfService;

        public UserController(IUserService userService, IPdfService pdfService)
        {
            _userService = userService;
            _pdfService = pdfService;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            return Ok(users);
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound($"User with ID {id} was not found.");
            }

            return Ok(user);
        }

        // GET: api/User/role/2
        [HttpGet("role/{roleId}")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersByRole(int roleId)
        {
            var users = await _userService.GetUsersByRoleAsync(roleId);

            return Ok(users);
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(
            [FromBody] CreateUserDto dto)
        {
            try
            {
                var user = await _userService.CreateUserAsync(dto);

                return CreatedAtAction(
                    nameof(GetUserById),
                    new { id = user.Id },
                    user
                );
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(
            int id,
            [FromBody] UpdateUserDto dto)
        {
            try
            {
                var user = await _userService.UpdateUserAsync(id, dto);

                if (user == null)
                {
                    return NotFound($"User with ID {id} was not found.");
                }

                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await _userService.DeleteUserAsync(id);

            if (!deleted)
            {
                return NotFound($"User with ID {id} was not found.");
            }

            return NoContent();
        }

        [HttpGet("pdf")]
        public async Task<IActionResult> GenerateUsersPdf()
        {
            var pdf = await _pdfService.GenerateUsersPdfAsync();

            return File(
                pdf,
                "application/pdf",
                "users.pdf"
            );
        }
    }
}

