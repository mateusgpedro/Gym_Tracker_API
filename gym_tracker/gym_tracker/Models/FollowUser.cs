using System.ComponentModel.DataAnnotations.Schema;
using gym_tracker.Infra.Users;
using Newtonsoft.Json;

namespace gym_tracker.Models;

public class FollowUser
{
    /// <summary>
    /// Id of the user who is following this user
    /// </summary>
    public Guid FollowerId { get; set; }
    
    /// <summary>
    /// Id of user that is being followed by this user 
    /// </summary>
    public Guid FollowingId { get; set; }
    
    /// <summary>
    /// User who is following this user
    /// </summary>
    public AppUser Follower { get; set; }
    /// <summary>
    /// User that is being followed by this user
    /// </summary>
    public AppUser Following { get; set; }
    
    /// <summary>
    /// Status of the follow request.
    /// </summary>
    public required bool PendingStatus { get; set; }
}
