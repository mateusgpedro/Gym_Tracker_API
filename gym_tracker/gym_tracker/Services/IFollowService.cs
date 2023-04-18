using gym_tracker.Infra.Users;

namespace gym_tracker.Services;

public interface IFollowService
{
    Task<bool> FollowUser(AppUser currentUser, AppUser followedUser, bool isPrivate);
    
    //Task<bool> Decline
}