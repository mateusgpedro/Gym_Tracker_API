using gym_tracker.Infra.Users;

namespace gym_tracker.Models;

public class Post
{
    public Guid Id { get; set; }
    public AppUser User { get; set; }
    public Guid UserId { get; set; } 
    public string Title { get; set; }
    public string? Text { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<Vote> Votes { get; set; }

    public Post(Guid userId, string title, string? text)
    {
        UserId = userId;
        Title = title;
        Text = text;
        Comments = new List<Comment>();
        Votes = new List<Vote>();
    }
}