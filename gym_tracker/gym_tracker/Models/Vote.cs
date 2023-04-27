using gym_tracker.Infra.Users;

namespace gym_tracker.Models;

public class Vote<TItem>
{
    public AppUser User { get; set; }
    public string UserId { get; set; }
    public TItem Item { get; set; }
    public string ItemId { get; set; }
    public bool IsUpvote { get; set; }
}