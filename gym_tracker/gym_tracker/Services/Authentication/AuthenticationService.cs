using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using gym_tracker.Infra.Users;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MimeKit.Text;

namespace gym_tracker.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _configuration;
    private IUrlHelper Url;
    private HttpRequest _request;
    //private readonly EmailTokenProvider<IdentityUser> _emailTokenProvider;

    public AuthenticationService(UserManager<AppUser> userManager, IConfiguration configuration, IUrlHelper url, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _configuration = configuration;
        Url = url;
        _request = httpContextAccessor.HttpContext.Request;
        //_emailTokenProvider = tokenProvider;
    }

    public async Task<string> CreateTokenAsync(string email, AppUser user)
    {
        var claims = await _userManager.GetClaimsAsync(user);
        var subject = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Email, email),
        });
        subject.AddClaims(claims);

        var key = Encoding.ASCII.GetBytes(_configuration["JwtBearerTokenSettings:SecretKey"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = subject,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = _configuration["JwtBearerTokenSettings:Audience"],
            Issuer = _configuration["JwtBearerTokenSettings:Issuer"],
            Expires = DateTime.UtcNow.AddDays(90)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return (tokenHandler.WriteToken(token));
    }

    public async Task<string> GenerateConfirmationCode(AppUser user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //var token = await _emailTokenProvider.GenerateAsync(UserManager<AppUser>.ConfirmEmailTokenPurpose, _userManager, user);
        return token;
    }
    
    public async Task<string> GenerateResetPassToken(AppUser user)
    {
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        // var token = await _emailTokenProvider.GenerateAsync(UserManager<AppUser>.ResetPasswordTokenPurpose, _userManager, user);
        return token;
    }
    
    public async Task<bool> SendEmailAsync(string emailFormat, string confirmationCode)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_configuration["MailSettings:From"]));
        email.To.Add(MailboxAddress.Parse(_configuration["MailSettings:From"]));
        email.Subject = "Test";
        email.Body = new TextPart(TextFormat.Html) { Text = emailFormat + "\n" + confirmationCode};

        var smtp = new SmtpClient();
        await smtp.ConnectAsync("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_configuration["MailSettings:From"], _configuration["MailSettings:Password"]);

        try
        {
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    
}