using BackendService.BusinessLogic.Operations.ChangePassword.Models;

namespace BackendService.BusinessLogic.Operations.ChangePassword;

public interface IChangePasswordOperation
{
    Task ChangeAsync(ChangePasswordOperationRequest request);
}