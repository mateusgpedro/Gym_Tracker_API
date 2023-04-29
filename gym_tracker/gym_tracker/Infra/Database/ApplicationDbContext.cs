using gym_tracker.Infra.Users;
using gym_tracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace gym_tracker.Infra.Database;

public class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Follow System

        #region Following System

        builder.Entity<FollowUser>()
            .HasKey(fu => new { fu.FollowerId, fu.FollowingId });

        builder.Entity<FollowUser>()
            .HasOne(fu => fu.Follower)
            .WithMany(u => u.Follower)
            .HasForeignKey(fu => fu.FollowingId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<FollowUser>()
            .HasOne(fu => fu.Following)
            .WithMany(u => u.Following)
            .HasForeignKey(fu => fu.FollowerId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        #region Blocking System

        // Blocking System
        builder.Entity<BlockUser>()
            .HasKey(bu => new { bu.BlockerId, bu.BlockingId });

        builder.Entity<BlockUser>()
            .HasOne(bu => bu.Blocker)
            .WithMany(u => u.Blocker)
            .HasForeignKey(bu => bu.BlockingId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<BlockUser>()
            .HasOne(bu => bu.Blocking)
            .WithMany(u => u.Blocking)
            .HasForeignKey(bu => bu.BlockerId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion
        
        #region Posting System

        builder.Entity<Post>(p =>
        {
            p.HasKey(p => p.Id);

            p.HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            p.Property(p => p.Title)
                .HasMaxLength(100);
            p.Property(p => p.Text)
                .HasMaxLength(1500);
        });

        builder.Entity<Comment>(c =>
        {
            c.HasKey(c => c.Id);
            
            c.HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            c.HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);
            
            c.Property(c => c.CommentText)
                .HasMaxLength(1500);
        });

        builder.Entity<Vote>(v =>
        {
            v.HasKey(v => v.Id);
            
            v.HasOne(v => v.User).
                WithMany(u => u.Votes)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            v.HasOne(v => v.Comment)
                .WithMany(c => c.Votes)
                .HasForeignKey(v => v.CommentId)
                .OnDelete(DeleteBehavior.Restrict);
            v.HasOne(v => v.Post)
                .WithMany(p => p.Votes)
                .HasForeignKey(v => v.PostId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        #endregion
    }

    public DbSet<Vote> Votes { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<FollowUser> FollowUsers { get; set; }
    public DbSet<BlockUser> BlockUsers { get; set; }
}