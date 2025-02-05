using System.Net;
using BackendService.BusinessLogic.Operations.AuthenticateUser;
using BackendService.BusinessLogic.Operations.AuthenticateUser.Models;
using BackendService.Contracts.AuthenticateUser;
using BackendService.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BackendService.UnitTests.Controllers.UserControllerTest;

[TestClass]
public sealed class UserControllerTest
{
    private readonly UserController _userController;
    private readonly Mock<IAuthenticateUserOperation> _authenticateUserOperation;

    public UserControllerTest()
    {
        _authenticateUserOperation = new Mock<IAuthenticateUserOperation>();

        _userController = new UserController(_authenticateUserOperation.Object);
    }

    [TestMethod]
    public async Task AddUserOperation_ReturnsActionResult()
    {
        _authenticateUserOperation
            .Setup(u => u.AuthenticateAsync(It.IsAny<AuthenticateUserOperationRequest>()))
            .ReturnsAsync(new AuthenticateUserOperationResponse(true));

        var response = await _userController.AuthenticateUserAsync(new AuthenticateUserRequest("login", "pass"))
            .ConfigureAwait(false);

        Assert.IsInstanceOfType(response, typeof(IActionResult));

        var result = response as OkObjectResult ?? throw new Exception("Cast Response to OkObjectResult return null");

        Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);

        var authenticationUserResponse = result.Value as AuthenticateUserResponse;

        Assert.IsTrue(authenticationUserResponse?.IsAuthenticated);
    }
}