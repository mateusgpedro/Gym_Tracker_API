using gym_tracker.Infra.Database;
using gym_tracker.Infra.Posts.Requests;
using gym_tracker.Infra.Users;
using gym_tracker.Models;
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

    public PostController(UserManager<AppUser> userManager, ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _dbContext = dbContext;
    }

    [Authorize]
    [HttpPost("create-post")]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
    {   
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
            return BadRequest("Failed to find user with the specified id");
        var post = new Post(request.UserId, request.Title, request.Text);

        if (user.Posts.IsNullOrEmpty())
            user.Posts = new List<Post>();
        
        user.Posts.Add(post);
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return Ok("Failed to save changes");
        }
        return Ok("Successfully created post");
    }

    [Authorize]
    [HttpPost("add-comment")]
    public async Task<ActionResult> AddComment(AddCommentRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        var post = _dbContext.Posts
            .FirstOrDefault(p => p.PostId == Guid.Parse(request.PostId));
        
        if (user == null)
            return BadRequest("Failed to find user with the specified id");
        if (post == null)
            return BadRequest("Failed to find post with specified id");

        var comment = new Comment(request.UserId, request.PostId, request.CommentText);

        user.Comments.Add(comment);
        post.Comments.Add(comment);
        
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return Ok("Failed to save changes");
        }

        await _dbContext.SaveChangesAsync();
        return Ok();
    }
    
}