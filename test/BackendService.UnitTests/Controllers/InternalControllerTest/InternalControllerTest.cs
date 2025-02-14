using System.Net;
using BackendService.BusinessLogic.Operations.AddUser;
using BackendService.BusinessLogic.Operations.ChangePassword;
using BackendService.Contracts.AddUser;
using BackendService.Contracts.ChangeUserPassword;
using BackendService.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BackendService.UnitTests.Controllers.InternalControllerTest;

[TestClass]
public sealed class InternalControllerTest
{
    private readonly InternalController _internalController;

    public InternalControllerTest()
    {
        Mock<IAddUserOperation> addUserOperation = new();
        Mock<IChangePasswordOperation> changePasswordOperation = new();

        _internalController = new InternalController(addUserOperation.Object, changePasswordOperation.Object);
    }

    [TestMethod]
    public async Task AddUserOperation_ReturnsActionResult()
    {
        var response = await _internalController.AddUserAsync(new AddUserRequest("login", "pass"))
            .ConfigureAwait(false);

        Assert.IsInstanceOfType(response, typeof(IActionResult));

        var result = response as OkResult ?? throw new Exception("Cast Response to OkObjectResult return null");

        Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
    }
    
    
    [TestMethod]
    public async Task ChangePasswordOperation_ReturnsActionResult()
    {
        var response = await _internalController.ChangePasswordAsync(new ChangePasswordRequest("login", "OldPass", "NewPass"))
            .ConfigureAwait(false);

        Assert.IsInstanceOfType(response, typeof(IActionResult));

        var result = response as OkResult ?? throw new Exception("Cast Response to OkObjectResult return null");

        Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
    }
}