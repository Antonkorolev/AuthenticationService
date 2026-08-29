using BackendService.BusinessLogic.Operations.AddUser.Models;
using BackendService.Contracts.AddUser;

namespace BackendService.Mappers;

public static class RequestMapper
{
    public static AddUserOperationRequest Map(this AddUserRequest request)
    {
        var roleInfos = request.Roles.Select(r => new RoleInfo(r.RoleCode, r.RestrictionType, r.RestrictionValue)).ToArray();
        return new AddUserOperationRequest(request.Login, request.Password, roleInfos);
    }
}