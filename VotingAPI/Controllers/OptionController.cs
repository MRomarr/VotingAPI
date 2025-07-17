using Microsoft.AspNetCore.Mvc;

namespace VotingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OptionController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOptionService _optionService;

        public OptionController(UserManager<ApplicationUser> userManager, IOptionService optionService)
        {
            _userManager = userManager;
            _optionService = optionService;
        }


        [HttpPost("create-option")]
        [Authorize]
        public async Task<IActionResult> CreateOption([FromBody] CreateOptionDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return NotFound("User Not Found");

            var result = await _optionService.CreateOptionAsync(user.Id, dto);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }


        [HttpPut("update-option")]
        [Authorize]
        public async Task<IActionResult> UpdateOption([FromBody] UpdateOptionDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return NotFound("User Not Found");

            var result = await _optionService.UpdateOptionAsync(user.Id, dto);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }


        [HttpDelete("delete-option")]
        [Authorize]
        public async Task<IActionResult> DeleteOption([FromQuery] DeleteOptionDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return NotFound("User Not Found");

            var result = await _optionService.DeleteOptionAsync(user.Id, dto);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }
    }
}
