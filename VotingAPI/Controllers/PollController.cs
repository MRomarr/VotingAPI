using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VotingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PollController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPollService _pollService;
        public PollController(UserManager<ApplicationUser> userManager, IPollService pollService)
        {
            _userManager = userManager;
            _pollService = pollService;
        }


        [HttpGet("get-all-polls")]
        [Authorize(Roles = RoleHelper.Admin)]
        public async Task<IActionResult> GetAllPolls()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return NotFound("User Not Found");

            var result = await _pollService.GetAllPollsAsync();

            return Ok(result);
        }

        [HttpGet("get-user-polls")]
        [Authorize]
        public async Task<IActionResult> GetUserPolls()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is null)
                return NotFound("User Not Found");

            var result = await _pollService.GetUserPollsAsync(user.Id);
            return Ok(result);
        }

        [HttpGet("get-poll-by-id/{Id}")]
        [Authorize(Roles = RoleHelper.Admin)]
        public async Task<IActionResult> GetPollById(string Id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is null)
                return NotFound("User Not Found");
            // in real app will send User ID To check if user have this poll or not
            var result = await _pollService.GetPollById(Id);
            if (!result.Success)
            {
                return NotFound(result.Message);
            }
            return Ok(result);
        }

        [HttpPost("create-poll")]
        [Authorize]
        public async Task<IActionResult> CreatePoll([FromBody] CreatePollDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return NotFound("User Not Found");

            var result = await _pollService.CreatePollAsync(user.Id, dto);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpPut("update-poll")]
        [Authorize]
        public async Task<IActionResult> UpdatePoll([FromBody] UpdatePollDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return NotFound("User Not Found");

            var result = await _pollService.UpdatePollAsync(user.Id, dto);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpDelete("delete-poll")]
        [Authorize]
        public async Task<IActionResult> DeletePoll(string Id)
        {
            var user = await _userManager.GetUserAsync (User);
            if (user is null)
                return NotFound("User Not Found");

            var result = await _pollService.DeletePollAsync(user.Id,Id);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }
    }
}
