using gym_tracker.Infra.Users;

namespace gym_tracker.Models;

public class VotingEntities
{
    public Guid Id { get; set; }
    public AppUser User { get; set; }
    public Guid UserId { get; set; } 
}