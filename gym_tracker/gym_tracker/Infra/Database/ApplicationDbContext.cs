using gym_tracker.Infra.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace gym_tracker.Infra.Database;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<AppUser>()
            .HasOne(u => u.FollowUser)
            .WithMany(f => f.Following)
            .HasForeignKey(e => e.Id);
    }

    private DbSet<FollowUser> FollowUsers { get; set; }
}