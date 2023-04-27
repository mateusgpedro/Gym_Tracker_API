using gym_tracker.Infra.Authentication;
using gym_tracker.Infra.Users;
using Microsoft.AspNetCore.Identity;

namespace gym_tracker.Services;

public interface IAuthenticationService
{
   Task<string> CreateTokenAsync(string email, AppUser user);

   Task<string> GenerateConfirmationCode(AppUser user);
   
   Task<string> GenerateResetPassToken(AppUser user);

   Task<bool> SendEmailAsync(string emailFormat, string confirmationCode);


}