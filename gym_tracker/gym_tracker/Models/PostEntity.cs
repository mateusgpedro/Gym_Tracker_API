using gym_tracker.Infra.Users;

namespace gym_tracker.Models;

public abstract class PostEntity
{
    public Guid Id { get; set; }
    public AppUser User { get; set; }
    public string UserId { get; set; }
}