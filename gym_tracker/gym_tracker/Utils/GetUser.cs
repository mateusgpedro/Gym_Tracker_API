using gym_tracker.Infra.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace gym_tracker.Utils;

public class GetUser
{
    public static async Task<AppUser> GetUserByIdWithBlockAndFollow(string userId, UserManager<AppUser> userManager)
    {
        return (await userManager.Users
            .Include(u => u.Blocker)
            .Include(u => u.Blocking)
            .Include(u => u.Follower)
            .Include(u => u.Following)
            .SingleOrDefaultAsync(u => u.Id == userId))!;
    }
    
    public static async Task<AppUser> GetUserByIdWithBlockerAndBlocking(string userId, UserManager<AppUser> userManager)
    {
        return (await userManager.Users
            .Include(u => u.Blocker)
            .Include(u => u.Blocking)
            .SingleOrDefaultAsync(u => u.Id == userId))!;
    }
    
    public static async Task<AppUser> GetUserByIdWithFollowersAndFollowing(string userId, UserManager<AppUser> userManager)
    {
        return (await userManager.Users
            .Include(u => u.Follower)
            .Include(u => u.Following)
            .SingleOrDefaultAsync(u => u.Id == userId))!;
    }

    public static async Task<AppUser> GetUserByIdWithPostsCommentsVotes(string userId, UserManager<AppUser> userManager)
    {
        return (await userManager.Users
            .Include(u => u.Posts)
            .Include(u => u.PostVotes)
            .Include(u => u.Comments)
            .Include(u => u.CommentVotes)
            .SingleOrDefaultAsync(u => u.Id == userId))!;
    }
}