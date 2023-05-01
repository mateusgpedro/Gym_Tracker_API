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
using Org.BouncyCastle.Ocsp;

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
        return Created($"/create-post/{post.Id}", post.Id);
    }

    [Authorize]
    [HttpPost("add-comment")]
    public async Task<ActionResult> AddComment(AddCommentRequest request)
    {
        var result = await _postService.CreateComment(request.UserId, request.PostId, request.CommentText);

        if (!result.Succeded)
            return BadRequest(result.Errors);
        
        await _dbContext.SaveChangesAsync();
        return Created($"/add-comment/{result.Item.Id}", result.Item.Id);
    }

    [Authorize]
    [HttpPost("add-vote")]
    public async Task<ActionResult> AddVote(VoteRequest request)
    {
        var postResult = new Result<Vote<Post>>();
        var commentResult = new Result<Vote<Comment>>();
        Guid userId;
        Guid itemId;
        var isUserValid = Guid.TryParse(request.UserId, out userId);
        var isItemValid = Guid.TryParse(request.ItemId, out itemId);
        var errors = new List<string>();

        if (!isUserValid)
            return BadRequest(ErrorMessages.InvalidUserGuid);
        if (!isItemValid)
            return BadRequest(ErrorMessages.InvalidItemGuid);

        var user = await GetUser.GetUserByIdWithPostsCommentsAndVotes(userId, _userManager);

        if (user == null)
            return BadRequest(ErrorMessages.UserNotFound);

        var hasVote = await _postService.HasVote(userId, itemId, _userManager);

        if (hasVote)
            return BadRequest("This user already");
        
        var post = await _dbContext.Posts.FirstOrDefaultAsync(p => p.Id == itemId);
        var comment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == itemId);

        if (post != null)
        {
            postResult = await _postService.CreatePostVote(user, post, request.IsUpvote);
            await _dbContext.SaveChangesAsync();
            return Created($"/add-vote/{postResult.Item.Id}", postResult.Item.Id);
        }
        else if (comment != null)
        {
            commentResult = await _postService.CreateCommentVote(user, comment, request.IsUpvote);
            await _dbContext.SaveChangesAsync();
            return Created($"/add-vote/{commentResult.Item.Id}", commentResult.Item.Id);
        }
        else
        {
            errors.Add(ErrorMessages.PostNotFound);
            errors.Add(ErrorMessages.CommentNotFound);
            return BadRequest(errors);
        }
    }
    
    //[Authorize]
    //[HttpPut]
}