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
    //private ISearchService _searchService;
    private UserManager<AppUser> _userManager;

    public SearchController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
        //_searchService = searchService;
        //_dbContext = dbContext;
    }
    
    [Authorize]
    [HttpGet("search-all")]
    public async Task<ActionResult<List<GetUsersResponse>>> SearchAllUsers([FromBody] SearchUserRequest? request)
    {
        if (request.Username is null)
            return Ok(Enumerable.Empty<AppUser>().ToList());
        //char[] delimitChars = { '.', '_', '-' };

        List<GetUsersResponse> users;
        
        if (request.Username.ContainsAny(' '))
            users = _userManager.Users.Where(u => u.FullName.Contains(request.Username))
                .Select(u => new GetUsersResponse(u.FullName, u.UserName))
                .ToList();
        else
            users = await _userManager.Users.Where(u => u.UserName.Contains(request.Username))
                .Select(u => new GetUsersResponse(u.FullName, u.UserName))
                .ToListAsync();

        if (users.IsNullOrEmpty())
            return Ok("No users found");
        return Ok(users.Take(6));
    }
}