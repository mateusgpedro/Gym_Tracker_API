namespace gym_tracker.Infra.Users;

public record SearchFollowRequest(string Username, string Id, int Page, int Rows);