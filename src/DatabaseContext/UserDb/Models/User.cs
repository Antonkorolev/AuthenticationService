namespace DatabaseContext.UserDb.Models;

public sealed class User
{
    public int UserId { get; set; }
    
    public string Login { get; set; } = default!;

    public string Password { get; set; } = default!;
}