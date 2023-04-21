namespace gym_tracker.Infra.Users;

public record DeclineFollowRequest(string CurrentUserId, string FollowerUserId);