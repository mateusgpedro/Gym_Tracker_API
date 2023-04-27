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
        if (currentUser.Following == null) 
            currentUser.Following = new List<FollowUser>();
        if (followedUser.Follower == null)
            followedUser.Follower = new List<FollowUser>();

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

    public async Task<bool> UnfollowUser(AppUser currentUser, AppUser followerUser, bool canSave)
    {
        currentUser = await GetUser.GetUserByIdWithFollowersAndFollowing(currentUser.Id, _userManager);

        var follow = currentUser.Following.FirstOrDefault(f => f.FollowingId == followerUser.Id);

        if (follow == null)
            return false;
        
        currentUser.Following.Remove(follow);
        followerUser.Follower.Remove(follow);
        if (canSave)
        {
            var result = await _userManager.UpdateAsync(currentUser);
            if (!result.Succeeded)
                return false;

            result = await _userManager.UpdateAsync(followerUser);
            if (!result.Succeeded)
                return false;
        }
        return true;
    }

    public async Task<bool> AcceptFollowRequest(AppUser currentUser, AppUser followerUser)
    {
        currentUser = await GetUser.GetUserByIdWithFollowersAndFollowing(currentUser.Id, _userManager);

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

    public async Task<bool> BlockUser(AppUser currentUser, AppUser blockedUser)
    {
        currentUser = await GetUser.GetUserByIdWithBlockAndFollow(currentUser.Id, _userManager);
        
        if (currentUser.Blocking == null)
            currentUser.Blocking = new List<BlockUser>();
        if (blockedUser.Blocker == null)
            blockedUser.Blocker = new List<BlockUser>();

        var block = new BlockUser
        {
            BlockerId = currentUser.Id,
            BlockingId = blockedUser.Id
        };
        
        currentUser.Blocking.Add(block);
        blockedUser.Blocker.Add(block);

        bool result = true;
        
        if (currentUser.Following.Any(f => f.FollowingId == blockedUser.Id))
            result = await UnfollowUser(currentUser, blockedUser, false);
        if (!result)
            return false;
        
        if (currentUser.Follower.Any(f => f.FollowerId == blockedUser.Id))
            result = await UnfollowUser(blockedUser, currentUser, false);
        if (!result)
            return false;

        var userResult = await _userManager.UpdateAsync(currentUser);
        if (!userResult.Succeeded)
            return false;
        
        userResult = await _userManager.UpdateAsync(blockedUser);
        if (!userResult.Succeeded)
            return false;
        
        return true;
    }

    public async Task<bool> UnblockUser(AppUser currentUser, AppUser blockedUser)
    {
        currentUser = await GetUser.GetUserByIdWithBlockerAndBlocking(currentUser.Id, _userManager);
        
        var block = currentUser.Blocking.FirstOrDefault(bu => bu.BlockingId == blockedUser.Id);

        if (block == null)
            return false;

        currentUser.Blocking.Remove(block);
        blockedUser.Blocker.Remove(block);
        var result = await _userManager.UpdateAsync(currentUser);
        if (!result.Succeeded)
            return false;

        result = await _userManager.UpdateAsync(blockedUser);
        if (!result.Succeeded)
            return false;
        return true;
    }
    
    
}