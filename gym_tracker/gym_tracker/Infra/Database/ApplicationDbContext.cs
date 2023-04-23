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

        // Blocking System
        builder.Entity<BlockUser>()
            .HasKey(bu => new { bu.BlockerId, bu.BlockingId });

        builder.Entity<BlockUser>()
            .HasOne(bu => bu.Blocker)
            .WithMany(u => u.Blocker)
            .HasForeignKey(bu => bu.BlockingId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<BlockUser>()
            .HasOne(bu => bu.Blocking)
            .WithMany(u => u.Blocking)
            .HasForeignKey(bu => bu.BlockerId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    public DbSet<FollowUser> FollowUsers { get; set; }
}