using gym_tracker.Infra.Users;
using Microsoft.AspNetCore.Identity;
using gym_tracker.Models;
using gym_tracker.Services;
using Microsoft.EntityFrameworkCore;

namespace gym_tracker.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class FollowController : ControllerBase
{
    private UserManager<AppUser> _userManager;
    private IFollowService _followService;

    public FollowController(UserManager<AppUser> userManager, IFollowService followService)
    {
        _userManager = userManager;
        _followService = followService;
    }

    [HttpPost("follow")]
    public async Task<ActionResult> FollowUser([FromBody] FollowRequest request)
    {
        var currentUser = await _userManager.FindByIdAsync(request.CurrentUserId);
        var followedUser = await _userManager.FindByIdAsync(request.FollowingUserId);

        if (currentUser == null || followedUser == null)
        {
            // handle the case where either user is not found
            return BadRequest("User not found");
        }
        
        var result = await _followService.FollowUser(currentUser, followedUser, followedUser.IsPrivate);
        return Ok();
    }

    [HttpDelete("decline-follow")]
    public async Task<ActionResult> DeclineFollowRequest(DeclineFollowRequest request)
    {
        var currentUser = await _userManager.FindByIdAsync(request.CurrentUserId);
        var followingUser = await _userManager.FindByIdAsync(request.FollowerUserId);

        if (currentUser == null || followingUser == null)
        {
            // handle the case where either user is not found
            return BadRequest("User not found");
        }

        var result = await _followService.DeclineFollowRequest(currentUser, followingUser);

        if (!result)
            return BadRequest("The specified user didn't request a follow on the current user");

        return Ok("Follow request declined");
    }

    [HttpGet("followers-count/{userId}")]
    public async Task<ActionResult<int>> GetFollowersNumber([FromRoute] string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return BadRequest("Failed to find user with the specific id");
        var count = await _userManager.Users
            .Where(u => u.Id == userId)
            .Select(u => u.Follower.Count)
            .FirstOrDefaultAsync();
        
        return Ok(count);
    }
    
    [HttpGet("following-count/{userId}")]
    public async Task<ActionResult<int>> GetFollowingNumber([FromRoute] string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return BadRequest("Failed to find user with the specific id");
        var count = await _userManager.Users
            .Where(u => u.Id == userId)
            .Select(u => u.Following.Count)
            .FirstOrDefaultAsync();

        return Ok(count);
    }
}