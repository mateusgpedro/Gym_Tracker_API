using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace gym_tracker.Infra.Users;

public class AppUser : IdentityUser
{
    public override string Id { get; set; }
    public required string FullName { get; set; }

    [ForeignKey("Follow")]
    public string FollowId;
    public FollowUser FollowUser { get; set; }
}