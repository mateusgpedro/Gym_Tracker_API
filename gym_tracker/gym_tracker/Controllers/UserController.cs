using gym_tracker.Infra.Authentication;
using gym_tracker.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using IAuthenticationService = gym_tracker.Services.IAuthenticationService;

namespace gym_tracker.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly EmailTokenProvider<IdentityUser> _emailTokenProvider;
    private SignInManager<IdentityUser> _signInManager;
    public UserController(UserManager<IdentityUser> userManager, IAuthenticationService authenticationService, EmailTokenProvider<IdentityUser> emailTokenProvider, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _authenticationService = authenticationService;
        _emailTokenProvider = emailTokenProvider;
        _signInManager = signInManager;
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser([FromBody] RegistrationRequest request)
    {
        var newUser = new IdentityUser { UserName = request.Username, Email = request.Email };
        var result = await _userManager.CreateAsync(newUser, request.Password);
        
        if (!result.Succeeded)
            return BadRequest(result.Errors.ConvertToProblemDetails());
        
        var confirmationCode = await _authenticationService.GenerateConfirmationCode(newUser);
        await _authenticationService.SendEmailAsync("Insert this code on the app", confirmationCode);
        return Created($"/register/{newUser.Id}", newUser.Id);
    }

    [AllowAnonymous]
    [HttpGet("login")]
    public async Task<ActionResult<string>> LoginUser([FromBody] LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        
        if (user == null)
            return BadRequest();
        if (!_userManager.CheckPasswordAsync(user, request.Password).Result)
            return BadRequest("Password invalid");
        
        if (!user.EmailConfirmed)
            return Unauthorized("Email confirmation needed");

        var token = await _authenticationService.CreateTokenAsync(request.Email, user);
        return Ok(token);
    }
    
    [AllowAnonymous]
    [HttpGet("confirm")]
    public async Task<ActionResult<string>> ConfirmEmail(ConfirmEmailRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return BadRequest("Failed to find user");

        var result = await _emailTokenProvider.ValidateAsync(UserManager<IdentityUser>.ConfirmEmailTokenPurpose, request.Code, _userManager, user);
        if (!result)
             return BadRequest("This code is not valid");

        var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        
        var confirmationResult = await _userManager.ConfirmEmailAsync(user, confirmationToken);

        if (!confirmationResult.Succeeded)
            return ValidationProblem(confirmationResult.Errors.ConvertToProblemDetails().ToString());
        var token = await _authenticationService.CreateTokenAsync(request.Email, user);

        return Ok(token);
    }

    [AllowAnonymous]
    [HttpPost("/request-reset")]
    public async Task<ActionResult> ResetPasswordEmail(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return BadRequest("Failed to find user");
        
        var token = await _authenticationService.GenerateResetPassToken(user);
        await _authenticationService.SendEmailAsync("Code to reset password:\n", token);
        return Ok();
    }

    [AllowAnonymous]
    [HttpPut("confirm-reset")]
    public async Task<ActionResult<string>> ConfirmResetPassword(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return BadRequest("Failed to find user");

        var result = await _emailTokenProvider.ValidateAsync(UserManager<IdentityUser>.ResetPasswordTokenPurpose,
            request.Code, _userManager, user);
        if (!result)
            return BadRequest("This code is not valid");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user); 
        
        var resetResult = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
        if (!resetResult.Succeeded)
            return resetResult.Errors.ConvertToProblemDetails().ToString();
        return Ok();
    }
}