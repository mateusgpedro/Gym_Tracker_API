using System.ComponentModel.DataAnnotations.Schema;

namespace gym_tracker.Infra.Users;

public class Follower
{
    [ForeignKey("Followers")]
    private string Id { get; set; }
    
    // Relations
    private List<AppUser>? Following { get; set; }
}