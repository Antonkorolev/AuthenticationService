using DatabaseContext.UserDb.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabaseContext.UserDb;

public class UserDbContext : DbContext, IUserDbContext
{
    public DbSet<User> User { get; set; }

    public UserDbContext(DbContextOptions options) : base(options)
    {
    }

    public void OnModelCreation(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}