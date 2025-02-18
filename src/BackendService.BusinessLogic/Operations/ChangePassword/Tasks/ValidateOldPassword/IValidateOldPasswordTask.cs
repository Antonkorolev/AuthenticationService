using BackendService.BusinessLogic.Operations.ChangePassword.Tasks.ValidateOldPassword.Models;

namespace BackendService.BusinessLogic.Operations.ChangePassword.Tasks.ValidateOldPassword;

public interface IValidateOldPasswordTask
{
    Task ValidateAsync(ValidateOldPasswordTaskRequest request);
}