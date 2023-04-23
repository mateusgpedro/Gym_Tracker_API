using gym_tracker.Infra.Users;
using Microsoft.AspNetCore.Identity;

namespace gym_tracker.Services;

public class ProfileService : IProfileService
{
    private UserManager<AppUser> _userManager;

    public ProfileService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    // public async Task<bool> SwitchAccountPrivacy(string userId)
    // {
    //     var user = await _userManager.FindByIdAsync(userId);
    //
    //     if (user == null)
    //         return "User not found";
    //     return ""
    // }
}