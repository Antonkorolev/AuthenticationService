using DatabaseContext.UserDb.Models;

namespace BackendService.BusinessLogic.Tasks.ValidateUser;

public interface IValidateUserTask
{
    Task<bool> ValidateAsync(string password, string hash);
}