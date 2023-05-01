using gym_tracker.Infra.Database;
using gym_tracker.Infra.Users;
using gym_tracker.Models;
using gym_tracker.Utils;
using Microsoft.AspNetCore.Identity;

namespace gym_tracker.Services.Posts;

public interface IPostService
{
    Task<Result<Comment>> CreateComment(string userId, string postId, string text);
    Task<Result<Vote<Post>>> CreatePostVote(AppUser user, Post post, bool isUpvote);
    Task<Result<Vote<Comment>>> CreateCommentVote(AppUser user, Comment comment, bool isUpvote);
    Task<bool> ChangeVote(string userId, string itemId);
    Task<bool> HasVote(Guid userId, Guid itemId, UserManager<AppUser> userManager);
}