using BackendService.BusinessLogic.Operations.AddUser.Models;

namespace BackendService.BusinessLogic.Operations.AddUser;

public interface IAddUserOperation
{
    Task AddAsync(AddUserOperationRequest request);
}