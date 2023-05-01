using gym_tracker.Infra.Users;

namespace gym_tracker.Models;

public class Post : VotingEntities
{
    public string Title { get; set; }
    public string? Text { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<Vote<Post>> Votes { get; set; }

    public Post(Guid userId, string title, string? text)
    {
        UserId = userId;
        Title = title;
        Text = text;
        Comments = new List<Comment>();
        Votes = new List<Vote<Post>>();
    }
}