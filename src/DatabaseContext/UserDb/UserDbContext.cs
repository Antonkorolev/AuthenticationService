using DatabaseContext.UserDb.Configurations;
using DatabaseContext.UserDb.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabaseContext.UserDb;

public class UserDbContext : DbContext, IUserDbContext
{
    public DbSet<User> User { get; set; }

    public DbSet<Settings> Settings { get; set; }

    public UserDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new SettingsConfiguration());
    }
}