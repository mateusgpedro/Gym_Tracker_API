using gym_tracker.Infra.Users;
using gym_tracker.Models;
using gym_tracker.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
            FollowingId = currentUser.Id,
            FollowerId = followedUser.Id,
            PendingStatus = isPrivate
        };
        
        currentUser.Following.Add(follow);
        followedUser.Follower.Add(follow);
        
        var result = await _userManager.UpdateAsync(currentUser);
        if (!result.Succeeded)
            return false;
        
        result = await _userManager.UpdateAsync(followedUser);
        if (!result.Succeeded)
            return false;
        
        return true;
    }

    
    public async Task<bool> DeclineFollowRequest(AppUser currentUser, AppUser followingUser)
    {
        // if (followingUser.Follower == null)
        //     followingUser.Follower = new List<FollowUser>();

        followingUser = await GetUserByIdWithFollowersAndFollowing(followingUser.Id);

        var followRequest = followingUser.Follower.FirstOrDefault(f => f.FollowerId == currentUser.Id);

        if (followRequest == null)
        {
            return false;
        }

        followingUser.Follower.Remove(followRequest);
        var result = await _userManager.UpdateAsync(followingUser);

        if (!result.Succeeded)
        {
            return false;
        }

        return true;
    }
    
    public async Task<int> GetFollowingCount(AppUser currentUser)
    {
        var count = await _userManager.Users
            .Where(u => u.Id == currentUser.Id)
            .Select(u => u.Following.Count)
            .FirstOrDefaultAsync();

        return count;
    }
    
    public async Task<int> GetFollowersCount(AppUser currentUser)
    {
        var count = await _userManager.Users
            .Where(u => u.Id == currentUser.Id)
            .Select(u => u.Follower.Count)
            .FirstOrDefaultAsync();

        return count;
    }
    
    public async Task<AppUser> GetUserByIdWithFollowersAndFollowing(string userId)
    {
        return await _userManager.Users
            .Include(u => u.Follower)
            .Include(u => u.Following)
            .SingleOrDefaultAsync(u => u.Id == userId);
    }
}