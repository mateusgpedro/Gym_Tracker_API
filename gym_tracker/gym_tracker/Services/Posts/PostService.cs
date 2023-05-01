using gym_tracker.Infra.Database;
using gym_tracker.Infra.Users;
using gym_tracker.Models;
using gym_tracker.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

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

    public async Task<Result<Comment>> CreateComment(string userId, string postId, string text)
    {
        var result = new Result<Comment>();
        
        var user = await _userManager.FindByIdAsync(userId);
        var post = await _dbContext.Posts
            .FirstOrDefaultAsync(p => p.Id == Guid.Parse(postId));

        if (user == null)
        {
            result.Errors.Add(ErrorMessages.UserNotFound);
            result.Succeded = false;
        }
        if (post == null)
        {
            result.Errors.Add(ErrorMessages.PostNotFound);
            result.Succeded = false;
        }

        if (!result.Succeded)
            return result;
        
        var comment = new Comment(Guid.Parse(userId), Guid.Parse(postId), text);

        result.Item = comment;

        user.Comments.Add(comment);
        post.Comments.Add(comment);
        
        return result;
    }

    public async Task<Result<Vote<Post>>> CreatePostVote(AppUser user, Post post, bool isUpvote)
    {
        var result = new Result<Vote<Post>>();

        if (user.PostVotes == null)
            user.PostVotes = new List<Vote<Post>>();

        Vote<Post> vote = new Vote<Post>()
        {
            ItemId = post.Id,
            IsUpvote = isUpvote
        };
        user.PostVotes.Add(vote);
        post.Votes.Add(vote); 
        _dbContext.Add(vote);
        
        if (!result.Succeded)
            return result;
        
        result.Item = vote;
        return result;
    }

    public async Task<Result<Vote<Comment>>> CreateCommentVote(AppUser user, Comment comment, bool isUpvote)
    {
        var result = new Result<Vote<Comment>>();

        if (user.CommentVotes == null)
            user.CommentVotes = new List<Vote<Comment>>();

        Vote<Comment> vote = new Vote<Comment>()
        {
            ItemId = comment.Id,
            IsUpvote = isUpvote
        };
        user.CommentVotes.Add(vote);
        comment.Votes.Add(vote);
        _dbContext.Add(vote);

        if (!result.Succeded)
            return result;
        
        result.Item = vote;
        return result;
    }
    
    public async Task<bool> ChangeVote(string userId, string itemId)
    {
        
        
        return true;
    }
    
    public async Task<bool> HasVote(Guid userId, Guid itemId, UserManager<AppUser> userManager)
    {
        var user = await GetUser.GetUserByIdWithPostsCommentsAndVotes(userId, userManager);
        var postVote = user.PostVotes.FirstOrDefault(u => u.Item.Id == itemId);
        var commentVote = user.CommentVotes.FirstOrDefault(u => u.Item.Id == itemId);

        if (commentVote != null || postVote != null)
            return true;
        
        return false;
    }
}