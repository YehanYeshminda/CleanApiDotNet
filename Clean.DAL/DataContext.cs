using Clean.DAL.Configurations;
using Clean.Domain.Aggregates.PostAggregate;
using Clean.Domain.Aggregates.UserProfileAggregate;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Clean.DAL;

public class DataContext : IdentityDbContext
{
    public DataContext()
    {
    }

    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Post> Posts { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
        {
            options.UseSqlServer("Server=localhost;Database=CleanArchitecture;User Id=sa;Password=bigStrongPwd@01;MultipleActiveResultSets=true;TrustServerCertificate=true");
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new PostCommentConfig());
        builder.ApplyConfiguration(new PostInteractionConfig());
        builder.ApplyConfiguration(new UserProfileConfig());
        builder.ApplyConfiguration(new IdentityUserLoginConfig());
        builder.ApplyConfiguration(new IdentityUserRoleConfig());
        builder.ApplyConfiguration(new IdentityUserTokenConfig());
    }
}