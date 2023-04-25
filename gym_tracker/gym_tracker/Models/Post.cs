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
    /// If of the post's owner
    /// </summary>
    public string UserId { get; set; }
    
    public string Title { get; set; }
    public string? Text { get; set; }
    public PostTag Tag { get; set; }

    public Post(string userId, string title, string? text, PostTag tag)
    {
        PostId = Guid.NewGuid();
        UserId = userId;
        Title = title;
        Text = text;
        Tag = tag;
    }
}

public enum PostTag
{
    None,
    Recipe,
    Training
}