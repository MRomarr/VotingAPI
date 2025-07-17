using Microsoft.AspNetCore.Mvc;

namespace VotingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        //delete vote
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVoteService _voteService;
        public VoteController(UserManager<ApplicationUser> userManager, IVoteService voteService)
        {
            _userManager = userManager;
            _voteService = voteService;
        }

        [HttpPost("add-vote")]
        [Authorize]
        public async Task<IActionResult> AddVote([FromBody] CreateVoteDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return NotFound("User Not Found");
            var result = await _voteService.AddVoteAsync(user.Id, dto);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpPut("update-vote")]
        [Authorize]
        public async Task<IActionResult> UpdateVote([FromBody] UpdateVoteDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return NotFound("User Not Found");
            var result = await _voteService.UpdateVoteAsync(user.Id, dto);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpDelete("delete-vote")]
        [Authorize]
        public async Task<IActionResult> DeleteVote([FromQuery] DeleteVoteDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return NotFound("User Not Found");
            var result = await _voteService.DeleteVoteAsync(user.Id, dto);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }
    }
}
