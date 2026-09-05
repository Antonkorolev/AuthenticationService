using BackendService.AuthorizationService.Contracts;
using BackendService.AuthorizationService.Contracts.Request;
using BackendService.BusinessLogic.Exceptions;
using BackendService.BusinessLogic.Mappers;
using BackendService.BusinessLogic.Operations.AuthenticateUser.Models;
using BackendService.BusinessLogic.Tasks.CreateJwtToken;
using BackendService.BusinessLogic.Tasks.GetHash;
using BackendService.BusinessLogic.Tasks.ValidatePassword;
using Microsoft.Extensions.Logging;

namespace BackendService.BusinessLogic.Operations.AuthenticateUser;

public sealed class AuthenticateUserOperation(
    IValidatePasswordTask validatePasswordTask,
    IGetHashTask getHashTask,
    IAuthorizationServiceClient authorizationServiceClient,
    ICreateJwtTokenTask createJwtTokenTask, 
    ILogger<AuthenticateUserOperation> logger) : IAuthenticateUserOperation
{
    public async Task<AuthenticateUserOperationResponse> AuthenticateAsync(AuthenticateUserOperationRequest request)
    {
        var getHashTaskResponse = await getHashTask.GetAsync(request.Login).ConfigureAwait(false);
        var isVerified = await validatePasswordTask.ValidateAsync(request.Password, getHashTaskResponse.Password).ConfigureAwait(false);

        if (!isVerified)
            throw new UserVerifiedException("User not verified");
        
        var getPermissionsResponse = await authorizationServiceClient.GetPermissionsAsync(new GetPermissionsRequest(request.Login)).ConfigureAwait(false);

        var token = await createJwtTokenTask.CreateAsync(getPermissionsResponse.Map(getHashTaskResponse.UserId)).ConfigureAwait(false);
        
        logger.LogInformation($"User authentication is {isVerified}. JWT token created");

        return new AuthenticateUserOperationResponse(token);
    }
}