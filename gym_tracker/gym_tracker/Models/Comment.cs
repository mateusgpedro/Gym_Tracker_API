using gym_tracker.Infra.Users;

namespace gym_tracker.Models;

public class Comment
{
    public Guid CommentId { get; set; }
    public AppUser User { get; set; }
    public string UserId { get; set; }
    public Post Post { get; set; }
    public string PostId { get; set; }
    public string CommentText { get; set; }

    public Comment(string userId, string postId, string commentText)
    {
        CommentId = Guid.NewGuid();
        UserId = userId;
        PostId = postId;
        CommentText = commentText;
    }
}
