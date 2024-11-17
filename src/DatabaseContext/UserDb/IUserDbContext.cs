using DatabaseContext.UserDb.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabaseContext.UserDb;

public interface IUserDbContext : IDataContext
{
    public DbSet<User> User { get; set; }
}