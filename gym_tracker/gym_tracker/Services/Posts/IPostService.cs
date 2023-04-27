using gym_tracker.Infra.Users;
using gym_tracker.Models;

namespace gym_tracker.Services.Posts;

public interface IPostService
{
    Task<bool> CreateComment(string userId, string postId, string text);
    Task<bool> CreateVote(string userId, string itemId, bool isUpvote);
    Task<bool> ChangeVote(string userId, string itemId);
}