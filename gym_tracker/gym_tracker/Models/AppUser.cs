using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using gym_tracker.Models;
using Microsoft.AspNetCore.Identity;

namespace gym_tracker.Infra.Users;

public class AppUser : IdentityUser
{
    public AppUser()
    {
        Follower = new List<FollowUser>();
        Following = new List<FollowUser>();

        Blocker = new List<BlockUser>();
        Blocking = new List<BlockUser>();

        Posts = new List<Post>();
        Comments = new List<Comment>();
    } 
    
    public override string Id { get; set; }
    public required string FullName { get; set; }
    
    // Follow system
    public ICollection<FollowUser> Follower { get; set; }
    public ICollection<FollowUser> Following { get; set; }
    
    public ICollection<BlockUser> Blocker { get; set; }
    public ICollection<BlockUser> Blocking { get; set; }

    public ICollection<Post> Posts { get; set; }
    public ICollection<Comment> Comments { get; set; }

    // Account Settings
    public bool IsPrivate { get; set; } = false;
}