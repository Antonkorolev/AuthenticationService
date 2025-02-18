using BackendService.BusinessLogic.Operations.ChangePassword.Tasks.ChangePassword.Models;

namespace BackendService.BusinessLogic.Operations.ChangePassword.Tasks.ChangePassword;

public interface IChangePasswordTask
{
   Task ChangeAsync(ChangePasswordRequest request);
}