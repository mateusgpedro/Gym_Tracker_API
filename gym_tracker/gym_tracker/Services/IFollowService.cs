using gym_tracker.Infra.Users;

namespace gym_tracker.Services;

public interface IFollowService
{
    Task FollowUser(FollowRequest request);
}