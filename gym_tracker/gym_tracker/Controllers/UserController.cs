using System.Security.Claims;
using gym_tracker.Infra.Authentication;
using gym_tracker.Services;
using gym_tracker.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Primitives;
using Org.BouncyCastle.Ocsp;

namespace gym_tracker.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UserController : ControllerBase
{
    private IRegistrationService _registrationService;
    private readonly UserManager<IdentityUser> _userManager;
    public UserController(UserManager<IdentityUser> userManager, IRegistrationService registrationService)
    {
        _userManager = userManager;
        _registrationService = registrationService;
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult> PostUser([FromBody]RegistrationRequest request)
    {
        var newUser = new IdentityUser { UserName = request.Username, Email = request.Email };
        var result = await _userManager.CreateAsync(newUser, request.Password);
        
        if (!result.Succeeded)
            return BadRequest(result.Errors.ConvertToProblemDetails());

        var loginRequest = new LoginRequest(request.Email, request.Password);
        var token = await _registrationService.CreateTokenAsync(loginRequest, newUser);
        var confirmationLink = await _registrationService.GenerateConfirmationLink(newUser);
        await _registrationService.SendEmailAsync("Hello, this is a test", confirmationLink);
        return Created($"/register/{newUser.Id}", token);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult> LoginUser(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        
        if (user == null)
            return BadRequest();
        if (!_userManager.CheckPasswordAsync(user, request.Password).Result)
            return BadRequest();
        
        var token = await _registrationService.CreateTokenAsync(request, user);
        return Created($"/register/{user.Id}", token);
    }
    
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult> ConfirmEmail(string token, string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return BadRequest("Failed to find user");
        
        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded)
            return BadRequest(result.Errors.ConvertToProblemDetails());
        
        return Ok();
    }
}