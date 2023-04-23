using gym_tracker.Infra.Users;

namespace gym_tracker.Services;

public interface IFollowService
{
    Task<bool> FollowUser(AppUser currentUser, AppUser followedUser, bool isPrivate);

    Task<bool> RemoveFollower(AppUser currentUser, AppUser followingUser);

    Task<bool> AcceptFollowRequest(AppUser currentUser, AppUser followerUser);

    Task<AppUser> GetUserByIdWithFollowersAndFollowing(string userId);
}