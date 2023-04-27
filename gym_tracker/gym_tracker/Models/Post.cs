using System.ComponentModel;
using gym_tracker.Infra.Users;

namespace gym_tracker.Models;

public class Post
{
    public Guid PostId { get; set; }
    
    /// <summary>
    /// Post's owner
    /// </summary>
    public AppUser User { get; set; }
    
    /// <summary>
    /// Id of the post's owner
    /// </summary>
    public string UserId { get; set; }
    
    public string Title { get; set; }
    public string? Text { get; set; }

    public int Votes { get; set; } = 0;

    public ICollection<Comment> Comments { get; set; }

    public Post(string userId, string title, string? text)
    {
        PostId = Guid.NewGuid();
        UserId = userId;
        Title = title;
        Text = text;
        Comments = new List<Comment>();
    }
}