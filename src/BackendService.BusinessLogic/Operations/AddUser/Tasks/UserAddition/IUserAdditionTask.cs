namespace BackendService.BusinessLogic.Operations.AddUser.Tasks.UserAddition;

public interface IUserAdditionTask
{
    Task AddAsync(string login, string hash);
}