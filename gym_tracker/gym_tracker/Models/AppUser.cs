using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using gym_tracker.Models;
using Microsoft.AspNetCore.Identity;

namespace gym_tracker.Infra.Users;

public class AppUser : IdentityUser
{
    public override string Id { get; set; }
    public required string FullName { get; set; }
    
    // Follow system
    public ICollection<FollowUser> Follower { get; set; }
    public ICollection<FollowUser> Following { get; set; }
    
    // Account Settings
    public bool IsPrivate { get; set; } = false;
}