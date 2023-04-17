using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gym_tracker.Infra.Users;

public class FollowUser
{
    public string FollowerId { get; set; }
    public string FollowingId { get; set; }

    public ICollection<AppUser> Following { get; set; }
}