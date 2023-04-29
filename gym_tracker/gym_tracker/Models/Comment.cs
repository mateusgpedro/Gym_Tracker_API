using gym_tracker.Infra.Users;

namespace gym_tracker.Models;

public class Comment
{
    public Guid Id { get; set; }
    public AppUser User { get; set; }
    public Guid UserId { get; set; } 
    public Post Post { get; set; }
    public Guid PostId { get; set; }
    public string CommentText { get; set; }
    public ICollection<Vote> Votes { get; set; }

    public Comment(Guid userId, Guid postId, string commentText)
    {
        UserId = userId;
        PostId = postId;
        CommentText = commentText;
        Votes = new List<Vote>();
    }
}
