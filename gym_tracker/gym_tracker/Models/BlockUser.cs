using gym_tracker.Infra.Users;

namespace gym_tracker.Models;

public class BlockUser
{
    public string BlockerId { get; set; }
    public string BlockingId { get; set; }

    public AppUser Blocker { get; set; }
    public AppUser Blocking { get; set; }
}