using gym_tracker.Infra.Database;
using gym_tracker.Infra.Users;
using gym_tracker.Infra.Users.Responses;
using gym_tracker.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace gym_tracker.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class SearchController : ControllerBase
{
    private UserManager<AppUser> _userManager;
    private ApplicationDbContext _dbContext;

    public SearchController(UserManager<AppUser> userManager, ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }
    
    [Authorize]
    [HttpGet("search-all")]
    public async Task<ActionResult<List<GetUsersResponse>>> SearchAllUsers([FromBody] SearchFollowRequest? request)
    {
        if (request.Username is null)
            return Ok(Enumerable.Empty<AppUser>().ToList());

        List<GetUsersResponse> users;
        
        if (request.Username.ContainsAny(' '))
            users = await _userManager.Users.Where(u => u.FullName.Contains(request.Username))
                .Select(u => new GetUsersResponse(u.FullName, u.UserName))
                .Skip((request.Page - 1) * request.Rows)
                .Take(request.Rows)
                .ToListAsync();
        else
            users = await _userManager.Users.Where(u => u.UserName.Contains(request.Username))
                .Select(u => new GetUsersResponse(u.FullName, u.UserName))
                .Skip((request.Page - 1) * request.Rows)
                .Take(request.Rows)
                .ToListAsync();

        if (users.IsNullOrEmpty())
            return Ok("No users found");
        return Ok(users);
    }

    [Authorize]
    [HttpGet("search-following")]
    public async Task<ActionResult<List<GetUsersResponse>>> SearchFollowing([FromBody] SearchFollowRequest? request)
    {
        if (request.Username is null)
            return Ok(Enumerable.Empty<AppUser>().ToList());

        List<GetUsersResponse> users;

        if (request.Username.ContainsAny(' '))
        {
            users = await _dbContext.FollowUsers
                .Include(u => u.Following)
                .Where(f => f.FollowerId == Guid.Parse(request.Id) && f.PendingStatus == false)
                .Select(f => f.Following)
                .Where(u => u.FullName == request.Username)
                .Select(u => new GetUsersResponse(u.FullName, u.UserName))
                .ToListAsync();
        }
        else
        {
            users = await _dbContext.FollowUsers
                .Include(u => u.Following)
                .Where(f => f.FollowerId == Guid.Parse(request.Id) && f.PendingStatus == false)
                .Select(f => f.Following)
                .Where(u => u.UserName == request.Username)
                .Select(u => new GetUsersResponse(u.FullName, u.UserName))
                .ToListAsync();
        }
        
        if (users.IsNullOrEmpty())
            return Ok();

        return Ok(users);
    }
    
    [Authorize]
    [HttpGet("search-followers")]
    public async Task<ActionResult<List<GetUsersResponse>>> SearchFollowers([FromBody] SearchFollowRequest? request)
    {
        if (request.Username is null)
            return Ok(Enumerable.Empty<AppUser>().ToList());

        List<GetUsersResponse> users;

        if (request.Username.ContainsAny(' '))
        {
            users = await _dbContext.FollowUsers
                .Include(u => u.Follower)
                .Where(f => f.FollowingId == Guid.Parse(request.Id) && f.PendingStatus == false)
                .Select(f => f.Follower)
                .Where(u => u.FullName == request.Username)
                .Select(u => new GetUsersResponse(u.FullName, u.UserName))
                .ToListAsync();
        }
        else
        {
            users = await _dbContext.FollowUsers
                .Include(u => u.Follower)
                .Where(f => f.FollowingId == Guid.Parse(request.Id) && f.PendingStatus == false)
                .Select(f => f.Follower)
                .Where(u => u.UserName == request.Username)
                .Select(u => new GetUsersResponse(u.FullName, u.UserName))
                .ToListAsync();
        }

        if (users.IsNullOrEmpty())
            return Ok();

        return Ok(users);
    }
}