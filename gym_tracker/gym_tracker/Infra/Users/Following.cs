using System.ComponentModel.DataAnnotations.Schema;

namespace gym_tracker.Infra.Users;

public class Following
{
    [ForeignKey("Following")]
    private string Id { get; set; }
    
    // Relations
    private List<AppUser>? Followers { get; set; }
}