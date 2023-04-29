using gym_tracker.Infra.Users;

namespace gym_tracker.Models;

public class Vote
{
    public Guid Id { get; set; }
    public bool IsUpvote { get; set; }
    
    public Guid UserId { get; set; }
    public readonly Guid? PostId;
    public readonly Guid? CommentId;
    
    public AppUser User { get; set; }
    public Comment Comment { get; set; }
    public Post Post { get; set; }

    public Vote(Guid? postId, Guid? commentId, bool isUpvote)
    {
        Id = Guid.NewGuid();
        PostId = postId;
        CommentId = commentId;
        IsUpvote = isUpvote;
    }
}