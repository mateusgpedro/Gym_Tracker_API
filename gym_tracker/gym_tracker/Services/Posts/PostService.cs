using gym_tracker.Infra.Database;
using gym_tracker.Infra.Users;
using gym_tracker.Models;
using gym_tracker.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PostEntity = gym_tracker.Models.PostEntity;

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

        var comment = new Comment(userId, postId, text);

        user.Comments.Add(comment);
        post.Comments.Add(comment);
        
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return false;
        }
        return true;
    }

    public async Task<bool> CreateVote(string userId, string itemId, bool isUpvote)
    {
        var user = await _userManager.FindByIdAsync(userId);
        var post = await _dbContext.Posts
            .FirstOrDefaultAsync(c => c.Id == Guid.Parse(itemId));
        var comment = await _dbContext.Comments
            .FirstOrDefaultAsync(c => c.Id == Guid.Parse(itemId));
        
        if (user == null)
            return false;

        if (post != null)
        {
            var vote = new Vote<Post>()
                { UserId = user.Id, ItemId = itemId, IsUpvote = isUpvote };
            user.PostVotes.Add(vote);
            post.Votes.Add(vote);
        }
        else if (comment != null)
        {
            var vote = new Vote<Comment>()
                { UserId = user.Id, ItemId = itemId, IsUpvote = isUpvote};
            user.CommentVotes.Add(vote);
            comment.Votes.Add(vote);
        }
        else
            return false;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return false;
        }
        return true;
    }

    public async Task<bool> ChangeVote(string userId, string itemId)
    {
        var user = await GetUser.GetUserByIdWithPostsCommentsVotes(userId, _userManager);
        var post = await _dbContext.Posts
            .FirstOrDefaultAsync(p => p.Id == Guid.Parse(itemId));
        var comment = await _dbContext.Comments
            .FirstOrDefaultAsync(c => c.Id == Guid.Parse(itemId));
        if (user == null)
            return false;

        if (post != null)
        {
            var vote = post.Votes.FirstOrDefault(v => v.UserId == userId);
            vote.IsUpvote = !vote.IsUpvote;
        }
        else if (comment != null)
        {
            var vote = post.Votes.FirstOrDefault(v => v.UserId == userId);
            vote.IsUpvote = !vote.IsUpvote;
        }
        else
            return false;
        
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return false;
        }
        
        return true;
    }
}