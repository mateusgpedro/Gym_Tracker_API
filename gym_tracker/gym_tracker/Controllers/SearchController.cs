using gym_tracker.Infra.Users;
using gym_tracker.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    public ActionResult<List<AppUser>> SearchAllUsers([FromBody] SearchUserRequest? request)
    {
        if (request.Username is null)
            return Ok(Enumerable.Empty<AppUser>().ToList());
        //char[] delimitChars = { '.', '_', '-' };

        List<AppUser> users;
        
        if (request.Username.ContainsAny(' '))
            users = _userManager.Users.Where(u => u.FullName.Contains(request.Username)).ToList();
        else
            users = _userManager.Users.Where(u => u.UserName.Contains(request.Username)).ToList();

        if (users.IsNullOrEmpty())
            return Ok("No users found");
        return Ok(users.Take(6));
    }
}