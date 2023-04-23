using gym_tracker.Infra.Users;

namespace gym_tracker.Services;

public interface IFollowService
{
    Task<bool> FollowUser(AppUser currentUser, AppUser followedUser, bool isPrivate);
    Task<bool> UnfollowUser(AppUser currentUser, AppUser followedUser, bool canSave);


    Task<bool> AcceptFollowRequest(AppUser currentUser, AppUser followerUser);

    Task<bool> BlockUser(AppUser currentUser, AppUser blockedUser);
    Task<bool> UnblockUser(AppUser currentUser, AppUser blockedUser);
    Task<AppUser> GetUserByIdWithBlockAndFollow(string userId);
    Task<AppUser> GetUserByIdWithBlockerAndBlocking(string userId);
    
    Task<AppUser> GetUserByIdWithFollowersAndFollowing(string userId);
}