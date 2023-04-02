using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using gym_tracker.Infra.Authentication;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MimeKit.Text;

namespace gym_tracker.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;
    private IUrlHelper Url;
    private HttpRequest _request;
    private readonly EmailTokenProvider<IdentityUser> _emailTokenProvider;

    public AuthenticationService(UserManager<IdentityUser> userManager, IConfiguration configuration, IUrlHelper url, IHttpContextAccessor httpContextAccessor, EmailTokenProvider<IdentityUser> tokenProvider)
    {
        _userManager = userManager;
        _configuration = configuration;
        Url = url;
        _request = httpContextAccessor.HttpContext.Request;
        _emailTokenProvider = tokenProvider;
    }

    public async Task<string> CreateTokenAsync(string email, IdentityUser user)
    {
        var claims = await _userManager.GetClaimsAsync(user);
        var subject = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
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

    public async Task<string> GenerateConfirmationCode(IdentityUser user)
    {
        var token = await _emailTokenProvider.GenerateAsync(UserManager<IdentityUser>.ConfirmEmailTokenPurpose, _userManager, user);
        // var confirmationLink = Url.Action(
        //                         "ConfirmEmail", 
        //                         "User",
        //                         new { token, userId = user.Id},
        //                         _request.Scheme);
        return token;
    }
    
    public async Task<bool> SendEmailAsync(string emailFormat, string confirmationLink)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_configuration["MailSettings:From"]));
        email.To.Add(MailboxAddress.Parse(_configuration["MailSettings:From"]));
        email.Subject = "Test";
        email.Body = new TextPart(TextFormat.Html) { Text = emailFormat + "\n" + confirmationLink};

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