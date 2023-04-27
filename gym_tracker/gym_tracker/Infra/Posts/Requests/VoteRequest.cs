namespace gym_tracker.Infra.Posts.Requests;

public record VoteRequest(string UserId, string ItemId, bool IsUpvote);