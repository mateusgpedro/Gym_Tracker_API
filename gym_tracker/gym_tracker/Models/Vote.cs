using gym_tracker.Infra.Users;

namespace gym_tracker.Models;

public class Vote<TItem> where TItem : VotingEntities
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? ItemId { get; set; }
    
    public bool IsUpvote { get; set; }

    public AppUser User { get; set; }
    public TItem Item { get; set; }

    public Vote()
    {
        Id = Guid.NewGuid();
    }
}