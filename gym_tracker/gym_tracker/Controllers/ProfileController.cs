using gym_tracker.Infra.Users;
using gym_tracker.Utils;
using Microsoft.AspNetCore.Identity;

namespace gym_tracker.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProfileController : ControllerBase
{
    private UserManager<AppUser> _userManager;

    public ProfileController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpPut("switch-privacy")]
    public async Task<ActionResult> SwitchProfilePrivacy(ProfilePrivacyRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.Id);
        
        if (user == null)
            return BadRequest("Failed to find the user with the specific Id");
        if (user.IsPrivate == request.IsPrivate)
            return BadRequest($"Profile is already {(request.IsPrivate ? "Private" : "Public")}");
        user.IsPrivate = request.IsPrivate;
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            return ValidationProblem(result.Errors.ConvertToProblemDetails().ToString());
        return Ok($"Switched profile to {(request.IsPrivate ? "Private" : "Public")}");
    }
}