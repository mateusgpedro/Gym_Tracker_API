using gym_tracker.Infra.Users;
using Microsoft.AspNetCore.Identity;
using gym_tracker.Models;
using gym_tracker.Services;

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
        var followedUser = await _userManager.FindByIdAsync(request.FollowedUserId);

        if (currentUser == null || followedUser == null)
        {
            // handle the case where either user is not found
            return BadRequest("User not found");
        }
        
        var result = await _followService.FollowUser(currentUser, followedUser, followedUser.IsPrivate);
        return Ok();
    }

    [HttpDelete("decline-follow")]
    public async Task<ActionResult> DeclineFollowRequest(FollowRequest request)
    {
        var currentUser = await _userManager.FindByIdAsync(request.CurrentUserId);
        var followedUser = await _userManager.FindByIdAsync(request.FollowedUserId);

        if (currentUser == null || followedUser == null)
        {
            // handle the case where either user is not found
            return BadRequest("User not found");
        }
    }
}