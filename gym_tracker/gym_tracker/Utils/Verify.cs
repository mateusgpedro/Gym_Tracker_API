using gym_tracker.Infra.Database;
using gym_tracker.Infra.Users;
using gym_tracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace gym_tracker.Utils;

public static class Verify
{
    public static async Task<bool> HasVote(Guid userId, Guid itemId, UserManager<AppUser> userManager, ApplicationDbContext dbContext)
    {
        var user = await GetUser.GetUserByIdWithPostsCommentsAndVotes(userId, userManager);
        var item = user.Votes.FirstOrDefault(u => u.Post.Id == itemId);
        item = user.Votes.FirstOrDefault(u => u.Comment.Id == itemId);

        if (item != null)
            return false;
        
        return true;
    }
}