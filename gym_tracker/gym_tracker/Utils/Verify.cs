using gym_tracker.Infra.Users;
using gym_tracker.Models;
using Microsoft.AspNetCore.Identity;

namespace gym_tracker.Utils;

public static class Verify
{
    public static async Task<bool> HasVote(string userId, string itemId, UserManager<AppUser> userManager)
    {
        var user = await GetUser.GetUserByIdWithPostsCommentsVotes(userId, userManager);
        
        var postVote = user.PostVotes.FirstOrDefault(p => p.ItemId == itemId);
        var commentVote = user.CommentVotes.FirstOrDefault(p => p.ItemId == itemId);
        
        if (postVote != null || commentVote != null)
            return false;

        return true;
    }
}