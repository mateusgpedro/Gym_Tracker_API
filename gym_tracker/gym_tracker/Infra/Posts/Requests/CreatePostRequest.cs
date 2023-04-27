using gym_tracker.Models;

namespace gym_tracker.Infra.Posts.Requests;

public record CreatePostRequest(string UserId, string Title, string? Text);