using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using VotingAPI.DTOs.AuthDTOs;

namespace VotingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.RegisterAsync(dto);
            
            if (!result.IsAuthenticated)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.LoginAsync(dto);

            if (!result.IsAuthenticated)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


        [HttpPost("assign-role")]
        [Authorize(Roles = RoleHelper.Admin)]
        public async Task<IActionResult> AddRole([FromBody] AddRoleDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.AddRoleAsync(dto);
            if (string.IsNullOrEmpty(result))
            {
                return BadRequest("Failed to add role.");
            }
            return Ok(result);
        }


        
    }
}
