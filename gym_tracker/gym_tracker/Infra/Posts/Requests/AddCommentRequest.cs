namespace gym_tracker.Infra.Posts.Requests;

public record AddCommentRequest(string UserId, string PostId, string CommentText);