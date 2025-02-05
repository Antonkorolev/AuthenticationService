using System.Net;
using BackendService.BusinessLogic.Operations.AddUser;
using BackendService.Contracts.AddUser;
using BackendService.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BackendService.UnitTests.Controllers.SupportControllerTest;

[TestClass]
public sealed class SupportControllerTest
{
    private readonly SupportController _supportController;

    public SupportControllerTest()
    {
        Mock<IAddUserOperation> addUserOperation = new();

        _supportController = new SupportController(addUserOperation.Object);
    }

    [TestMethod]
    public async Task AddUserOperation_ReturnsActionResult()
    {
        var response = await _supportController.AddUserAsync(new AddUserRequest("login", "pass"))
            .ConfigureAwait(false);

        Assert.IsInstanceOfType(response, typeof(IActionResult));

        var result = response as OkResult ?? throw new Exception("Cast Response to OkObjectResult return null");

        Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
    }
}