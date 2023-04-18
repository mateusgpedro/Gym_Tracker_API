using gym_tracker.Infra.Users;
using gym_tracker.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace gym_tracker.Infra.Database;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Follow System
        builder.Entity<FollowUser>()
            .HasKey(fu => new { fu.FollowerId, fu.FollowingId });

        builder.Entity<FollowUser>()
            .HasOne(fu => fu.Follower)
            .WithMany(u => u.Follower)
            .HasForeignKey(fu => fu.FollowingId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<FollowUser>()
            .HasOne(fu => fu.Following)
            .WithMany(u => u.Following)
            .HasForeignKey(fu => fu.FollowerId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private DbSet<FollowUser> FollowUsers { get; set; }
}