using gym_tracker.Infra.Database;
using gym_tracker.Infra.Posts.Requests;
using gym_tracker.Infra.Users;
using gym_tracker.Models;
using gym_tracker.Services.Posts;
using gym_tracker.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace gym_tracker.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class PostController : ControllerBase
{
    private ApplicationDbContext _dbContext;
    private UserManager<AppUser> _userManager;
    private IPostService _postService;

    public PostController(UserManager<AppUser> userManager, ApplicationDbContext dbContext, IPostService postService)
    {
        _userManager = userManager;
        _dbContext = dbContext;
        _postService = postService;
    }

    [Authorize]
    [HttpPost("create-post")]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
    {   
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
            return BadRequest("Failed to find user with the specified id");
        var post = new Post(Guid.Parse(request.UserId), request.Title, request.Text);

        if (user.Posts.IsNullOrEmpty())
            user.Posts = new List<Post>();
        
        user.Posts.Add(post);
        
        await _dbContext.SaveChangesAsync();
        return Ok("Successfully created post");
    }

    [Authorize]
    [HttpPost("add-comment")]
    public async Task<ActionResult> AddComment(AddCommentRequest request)
    {
        var result = await _postService.CreateComment(request.UserId, request.PostId, request.CommentText);

        if (!result)
            return BadRequest("Failed to create comment");
        
        await _dbContext.SaveChangesAsync();
        return Ok("Successfully commented");
    }

    [Authorize]
    [HttpPost("add-vote")]
    public async Task<ActionResult> AddVote(VoteRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
            return BadRequest("Failed to find user with the specified id");
        if (user.Votes == null)
            user.Votes = new List<Vote>();

        bool hasVoted = await Verify.HasVote(user.Id, Guid.Parse(request.ItemId), _userManager, _dbContext);
        bool result;

        if (hasVoted)
            result = await _postService.ChangeVote(request.UserId, request.ItemId);
        else
            result = await _postService.CreateVote(request.UserId, request.ItemId, request.IsUpvote);

        if (!result)
            return BadRequest("Failed to vote");
        
        await _dbContext.SaveChangesAsync();
        return Ok("Successfully voted");
    }
}