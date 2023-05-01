namespace gym_tracker.Utils;

public class ErrorMessages
{
    #region User

    public static readonly string UserNotFound = "Failed to find user with specified id";
    public static readonly string InvalidUserGuid = "Invalid user id format";
    
    #endregion

    #region Items
    
    public static readonly string InvalidItemGuid = "Invalid item id format";

    #endregion
    
    #region Post

    public static readonly string PostNotFound = "Failed to find post with specified id";
    public static readonly string CommentNotFound = "Failed to find comment with specified id";
    public static readonly string VoteNotFound = "Failed to find vote with specified id";
    public static readonly string ExistingVote = "Failed to find vote with specified id";

    #endregion
}