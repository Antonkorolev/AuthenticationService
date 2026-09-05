namespace BackendService.BusinessLogic.Tasks.GetHash.Models;

public sealed class GetHashTaskResponse(int userId, string password)
{
    public int UserId { get; } = userId;

    public string Password { get; } = password;
}