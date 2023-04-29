using gym_tracker.Infra.Users;
using gym_tracker.Models;
using Microsoft.AspNetCore.Identity;

namespace gym_tracker.Utils;

public static class Verify
{
    public static async Task<bool> HasVote(Guid userId, Guid itemId, UserManager<AppUser> userManager)
    {
        
        
        return true;
    }
}