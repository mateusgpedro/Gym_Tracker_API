using gym_tracker.Infra.Authentication;
using Microsoft.AspNetCore.Identity;

namespace gym_tracker.Services;

public interface IRegistrationService
{
   Task<string> CreateTokenAsync(LoginRequest model, IdentityUser user);

   Task<string> GenerateConfirmationLink(IdentityUser user);
   
   Task SendEmailAsync(string emailFormat, string confirmationLink);
   
}