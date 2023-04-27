using gym_tracker.Infra.Users;

namespace gym_tracker.Models;

public class Comment : PostEntity
{
    public Guid Id { get; set; }
    public AppUser User { get; set; }
    public string UserId { get; set; }
    public Post Post { get; set; }
    public string PostId { get; set; }
    public string CommentText { get; set; }
    public ICollection<Vote<Comment>> Votes { get; set; }

    public Comment(string userId, string postId, string commentText)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        PostId = postId;
        CommentText = commentText;
    }
}
