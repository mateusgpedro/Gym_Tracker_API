using gym_tracker.Infra.Users;
using Microsoft.AspNetCore.Identity;

namespace gym_tracker.Services;

public class FollowService : IFollowService
{
    private UserManager<AppUser> _userManager;

    public FollowService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task FollowUser(FollowRequest request)
    {
        var currentUser = await _userManager.FindByIdAsync(request.CurrentUserId);
        var followedUserId = await _userManager.FindByIdAsync(request.FollowedUserId);

        //currentUser.FollowingId;
    }
}