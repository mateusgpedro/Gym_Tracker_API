using gym_tracker.Infra.Database;
using gym_tracker.Infra.Users;
using gym_tracker.Models;
using gym_tracker.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace gym_tracker.Services.Posts;

public class PostService : IPostService
{
    private UserManager<AppUser> _userManager;
    private ApplicationDbContext _dbContext;

    public PostService(UserManager<AppUser> userManager, ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }

    public async Task<bool> CreateComment(string userId, string postId, string text)
    {
        var user = await _userManager.FindByIdAsync(userId);
        var post = await _dbContext.Posts
            .FirstOrDefaultAsync(p => p.Id == Guid.Parse(postId));

        if (user == null)
            return false;
        if (post == null)
            return false;

        var comment = new Comment(Guid.Parse(userId), Guid.Parse(postId), text);

        user.Comments.Add(comment);
        post.Comments.Add(comment);
        
        return true;
    }

    public async Task<bool> CreateVote(string userId, string itemId, bool isUpvote)
    {
        var user = await GetUser.GetUserByIdWithPostsCommentsAndVotes(Guid.Parse(userId), _userManager);

        if (user == null)
            return false;
        
        var post = await _dbContext.Posts.FirstOrDefaultAsync(p => p.Id == Guid.Parse(itemId));
        var comment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == Guid.Parse(itemId));

        var vote = new Vote(post.Id, comment.Id, isUpvote);

        if (post != null)
        {
            user.Votes.Add(vote);
            post.Votes.Add(vote);
        }
        else if (comment != null)
        {
            user.Votes.Add(vote);
            comment.Votes.Add(vote);
        }
        else
            return false;
        return true;
    }

    public async Task<bool> ChangeVote(string userId, string itemId)
    {
        throw new NotImplementedException();
    }
}