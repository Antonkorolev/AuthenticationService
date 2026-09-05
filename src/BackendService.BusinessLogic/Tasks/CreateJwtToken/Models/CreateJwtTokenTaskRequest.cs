namespace BackendService.BusinessLogic.Tasks.CreateJwtToken.Models;

public sealed class CreateJwtTokenTaskRequest(int userId, IEnumerable<RoleInfo> roleInfos)
{
    public int UserId { get; } = userId;

    public IEnumerable<RoleInfo> RoleInfos { get; } = roleInfos;
}