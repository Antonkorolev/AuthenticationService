using BackendService.AuthorizationService.Contracts.Response;
using BackendService.BusinessLogic.Tasks.CreateJwtToken.Models;
using Permission = BackendService.BusinessLogic.Tasks.CreateJwtToken.Models.Permission;

namespace BackendService.BusinessLogic.Mappers;

public static class RequestMapper
{
    public static CreateJwtTokenTaskRequest Map(this GetPermissionsResponse response, int userId)
    {
        var roleInfos = response.RolePermissions
            .Select(r => new RoleInfo(r.RoleCode, r.Permissions
                .Select(p => new Permission(p.PermissionCode, p.Restrictions?.RestrictionTypeCode, p.Restrictions?.RestrictionValue))))
            .ToArray();

        return new CreateJwtTokenTaskRequest(userId, roleInfos);
    }
}