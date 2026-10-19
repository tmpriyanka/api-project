using Microsoft.AspNetCore.Mvc;
using myProject02.Dto;
using myProject02.Services;

namespace myProject02.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateRole([FromBody] CreateRoleDTO createRoleDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdRole = await _roleService.CreateRoleAsync(createRoleDto);

            if (createdRole == null)
            {
                return BadRequest("A role with this name already exists");
            }
            return CreatedAtAction(nameof(GetRoleById), new { id = createdRole.Id }, createdRole);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(int id) {

            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null) return NotFound("Role not found.");

            return Ok(role);
        }
    }
}
