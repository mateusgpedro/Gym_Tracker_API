using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using gym_tracker.Infra.Authentication;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MimeKit.Text;

namespace gym_tracker.Services;

public class RegistrationService : IRegistrationService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;
    private IUrlHelper Url;
    private HttpRequest Request;

    public RegistrationService(UserManager<IdentityUser> userManager, IConfiguration configuration, IUrlHelper url, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _configuration = configuration;
        Url = url;
        Request = httpContextAccessor.HttpContext.Request;
    }

    public async Task<string> CreateTokenAsync(LoginRequest model, IdentityUser user)
    {
        var claims = await _userManager.GetClaimsAsync(user);
        var subject = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Email, model.Email),
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
            Expires = DateTime.UtcNow.AddDays(60)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return (tokenHandler.WriteToken(token));
    }

    public async Task<string> GenerateConfirmationLink(IdentityUser user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmationLink = Url.Action(
                                "ConfirmEmail", 
                                "User",
                                new { token, email = user.Email},
                                Request.Scheme);
        return confirmationLink;
    }
    
    public async Task SendEmailAsync(string emailFormat, string confirmationLink)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_configuration["MailSettings:From"]));
        email.To.Add(MailboxAddress.Parse(_configuration["MailSettings:From"]));
        email.Subject = "Test";
        email.Body = new TextPart(TextFormat.Html) { Text = emailFormat + "\n" + confirmationLink};

        var smtp = new SmtpClient();
        await smtp.ConnectAsync("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_configuration["MailSettings:From"], _configuration["MailSettings:Password"]);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}