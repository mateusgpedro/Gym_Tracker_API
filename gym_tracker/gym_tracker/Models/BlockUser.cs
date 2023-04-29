using gym_tracker.Infra.Users;

namespace gym_tracker.Models;

public class BlockUser
{
    public Guid BlockerId { get; set; }
    public Guid BlockingId { get; set; }

    public AppUser Blocker { get; set; }
    public AppUser Blocking { get; set; }
}