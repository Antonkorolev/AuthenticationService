using DatabaseContext.UserDb.Models;

namespace BackendService.BusinessLogic.Operations.AuthenticateUser.Tasks.ValidateUser;

public interface IValidateUserTask
{
    Task<bool> ValidateAsync(User? user, string login, string password);
}