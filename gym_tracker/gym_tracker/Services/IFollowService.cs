using gym_tracker.Infra.Users;

namespace gym_tracker.Services;

public interface IFollowService
{
    Task<bool> FollowUser(AppUser currentUser, AppUser followedUser, bool isPrivate);

    Task<bool> DeclineFollowRequest(AppUser currentUser, AppUser? followerUser);
    
    Task<int> GetFollowersCount(AppUser currentUser);
    Task<int> GetFollowingCount(AppUser userId);

    Task<AppUser> GetUserByIdWithFollowersAndFollowing(string userId);
}