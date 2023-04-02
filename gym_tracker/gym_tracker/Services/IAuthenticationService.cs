using gym_tracker.Infra.Authentication;
using Microsoft.AspNetCore.Identity;

namespace gym_tracker.Services;

public interface IAuthenticationService
{
   Task<string> CreateTokenAsync(string email, IdentityUser user);

   Task<string> GenerateConfirmationCode(IdentityUser user);

   Task<bool> SendEmailAsync(string emailFormat, string confirmationLink);

}