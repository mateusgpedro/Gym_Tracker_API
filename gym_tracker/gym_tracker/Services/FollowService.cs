using gym_tracker.Infra.Users;
using gym_tracker.Models;
using gym_tracker.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace gym_tracker.Services;

public class FollowService : IFollowService
{
    private UserManager<AppUser> _userManager;
    
    public FollowService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> FollowUser(AppUser currentUser, AppUser followedUser, bool isPrivate)
    {
        if (currentUser.Following == null) {
            currentUser.Following = new List<FollowUser>();
        }
        
        var follow = new FollowUser
        {
            FollowerId = currentUser.Id,
            FollowingId = followedUser.Id,
            PendingStatus = isPrivate
        };
        
        currentUser.Following.Add(follow);
        var result = await _userManager.UpdateAsync(currentUser);
        if (!result.Succeeded)
            return false;
        return true;
    }
    
}