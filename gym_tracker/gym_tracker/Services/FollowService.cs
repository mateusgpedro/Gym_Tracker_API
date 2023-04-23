using gym_tracker.Infra.Database;
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
    private ApplicationDbContext _dbContext;
    
    public FollowService(UserManager<AppUser> userManager, ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
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
    
    public async Task<bool> RemoveFollower(AppUser currentUser, AppUser followerUser)
    {
        followerUser = await GetUserByIdWithFollowersAndFollowing(followerUser.Id);

        var followRequest = followerUser.Follower.FirstOrDefault(f => f.FollowerId == currentUser.Id);

        if (followRequest == null)
        {
            return false;
        }

        followerUser.Follower.Remove(followRequest);
        var result = await _userManager.UpdateAsync(followerUser);

        if (!result.Succeeded)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> AcceptFollowRequest(AppUser currentUser, AppUser followerUser)
    {
        currentUser = await GetUserByIdWithFollowersAndFollowing(currentUser.Id);

        var followRequest = followerUser.Following.FirstOrDefault(f => f.FollowingId == currentUser.Id);

        if (followRequest == null)
            return false;
        
        followRequest.PendingStatus = false;
        var result = await _userManager.UpdateAsync(followerUser);

        if (!result.Succeeded)
        {
            return false;
        }
        
        return true;
    }
    
    public async Task<AppUser> GetUserByIdWithFollowersAndFollowing(string userId)
    {
        return await _userManager.Users
            .Include(u => u.Follower)
            .Include(u => u.Following)
            .SingleOrDefaultAsync(u => u.Id == userId);
    }
}