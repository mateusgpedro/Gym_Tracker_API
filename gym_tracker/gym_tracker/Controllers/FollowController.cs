using System.Text.Json;
using System.Text.Json.Serialization;
using gym_tracker.Infra.Database;
using gym_tracker.Infra.Users;
using gym_tracker.Infra.Users.Responses;
using Microsoft.AspNetCore.Identity;
using gym_tracker.Models;
using gym_tracker.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace gym_tracker.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class FollowController : ControllerBase
{
    private UserManager<AppUser> _userManager;
    private IFollowService _followService;
    private ApplicationDbContext _dbContext;

    public FollowController(UserManager<AppUser> userManager, IFollowService followService, ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _followService = followService;
        _dbContext = dbContext;
    }

    [HttpPost("follow")]
    public async Task<ActionResult> FollowUser([FromBody] FollowRequest request)
    {
        if (request.CurrentUserId == request.FollowingUserId)
            BadRequest("The user cannot follow himself");
        
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
    public async Task<ActionResult> DeclineOrRemoveFollower(FollowActionRequest request)
    {
        var currentUser = await _userManager.FindByIdAsync(request.CurrentUserId);
        var followingUser = await _userManager.FindByIdAsync(request.FollowerUserId);

        if (currentUser == null || followingUser == null)
        {
            // handle the case where either user is not found
            return BadRequest("User not found");
        }

        var result = await _followService.RemoveFollower(currentUser, followingUser);

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
            .SelectMany(u => u.Follower)
            .CountAsync(u => u.PendingStatus == false);
        
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
            .SelectMany(u => u.Following)
            .CountAsync(u => u.PendingStatus == false);

        return Ok(count);
    }

    [HttpGet("follow-requests/{userId}")]
    public async Task<ActionResult> GetFollowRequests([FromRoute] string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
    
        if (user == null)
            return BadRequest("Failed to find user with the specific id");
        
        var requests = await _dbContext.FollowUsers
            .Include(f => f.Follower)
            .Where(f => f.FollowingId == userId && f.PendingStatus == true)
            .Select(f => f.Follower)
            .Select(f => new GetUsersResponse(f.FullName, f.UserName))
            .ToListAsync();
        
        if (requests.IsNullOrEmpty())
            return Ok("You don't have any follow requests");

        return Ok(requests);
    }

    [HttpPut("accept-follow")]
    public async Task<ActionResult> AcceptFollowRequest(FollowActionRequest request)
    {
        var currentUser = await _userManager.FindByIdAsync(request.CurrentUserId);
        var followerUser = await _userManager.FindByIdAsync(request.FollowerUserId);

        if (currentUser == null || followerUser == null)
            return BadRequest("Failed to find user with the specific id");

        var result = await _followService.AcceptFollowRequest(currentUser, followerUser);
        
        if (!result)
            return BadRequest("The specified user didn't request a follow on the current user");
        
        return Ok();
    }

    [HttpGet("is-following")]
    public async Task<ActionResult<bool>> GetIfIsFolloingwUser(FollowActionRequest request)
    {
        var currentUser = await _followService.GetUserByIdWithFollowersAndFollowing(request.CurrentUserId);
        
        if (currentUser == null)
            return BadRequest("Failed to find user with the specific id");
        
        return Ok(currentUser.Following.Any(f => f.FollowingId == request.FollowerUserId && f.PendingStatus == false));
    }
    
    [HttpGet("user-followers/{userId}")]

    public async Task<ActionResult> GetWhoAreFollowers(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
    
        if (user == null)
            return BadRequest("Failed to find user with the specific id");

        var follower = await _dbContext.FollowUsers
            .Include(f => f.Follower)
            .Where(f => f.FollowingId == userId && f.PendingStatus == false)
            .Select(f => f.Follower)
            .Select(f => new GetUsersResponse(f.FullName, f.UserName))
            .ToListAsync();

        if (follower.IsNullOrEmpty())
            return Ok("User");
        
        return Ok(follower);
    }
    
    [HttpGet("user-following/{userId}")]

    public async Task<ActionResult> GetWhoYouFollowing(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
    
        if (user == null)
            return BadRequest("Failed to find user with the specific id");

        var follower = await _dbContext.FollowUsers
            .Include(f => f.Following)
            .Where(f => f.FollowerId == userId && f.PendingStatus == false)
            .Select(f => f.Following)
            .Select(f => new GetUsersResponse(f.FullName, f.UserName))
            .ToListAsync();

        if (follower.IsNullOrEmpty())
            return Ok("User");
        
        return Ok(follower);
    }
}