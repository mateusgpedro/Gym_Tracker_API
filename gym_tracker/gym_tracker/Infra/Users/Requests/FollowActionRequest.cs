namespace gym_tracker.Infra.Users;

public record FollowActionRequest(string CurrentUserId, string FollowerUserId);