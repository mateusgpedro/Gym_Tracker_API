using System.ComponentModel;
using gym_tracker.Infra.Users;

namespace gym_tracker.Models;

public class Post : PostEntity
{
    public string Title { get; set; }
    public string? Text { get; set; }

    public ICollection<Vote<Post>> Votes { get; set; }

    public ICollection<Comment> Comments { get; set; }

    public Post(string userId, string title, string? text)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Title = title;
        Text = text;
        Comments = new List<Comment>();
    }
}