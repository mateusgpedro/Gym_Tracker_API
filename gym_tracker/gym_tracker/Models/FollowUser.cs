using System.ComponentModel.DataAnnotations.Schema;
using gym_tracker.Infra.Users;

namespace gym_tracker.Models;

public class FollowUser
{
    public string FollowerId { get; set; }
    public string FollowingId { get; set; }

    public required bool PendingStatus { get; set; } = false;

    public AppUser Follower { get; set; }
    public AppUser Following { get; set; }
}