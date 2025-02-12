using BackendService.BusinessLogic.Operations.AddUser;
using BackendService.BusinessLogic.Operations.AddUser.Models;
using BackendService.BusinessLogic.Operations.AddUser.Tasks.UserAddition;
using BackendService.BusinessLogic.Tasks.GetSalt;
using BackendService.BusinessLogic.Tasks.GetSettings;
using BackendService.BusinessLogic.Tasks.GetSettings.Models;
using BackendService.BusinessLogic.Tasks.HashPassword;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BackendService.BusinessLogic.UnitTests.AuthenticateUser;

[TestClass]
public sealed class AddUserOperationTests
{
    private Mock<IHashPasswordTask> _hashPasswordTask = default!;
    private Mock<IUserAdditionTask> _userAdditionTask = default!;
    private Mock<IGetSaltTask> _getSaltTask = default!;
    private Mock<IGetSettingsTask> _getSettingsTask = default!;
    private Mock<ILogger<AddUserOperation>> _logger = default!;
    private AddUserOperation _addUserOperation = default!;

    [TestInitialize]
    public void TestInitialize()
    {
        _hashPasswordTask = new Mock<IHashPasswordTask>();
        _userAdditionTask = new Mock<IUserAdditionTask>();
        _getSaltTask = new Mock<IGetSaltTask>();
        _getSettingsTask = new Mock<IGetSettingsTask>();
        _logger = new Mock<ILogger<AddUserOperation>>();

        _addUserOperation = new AddUserOperation(
            _hashPasswordTask.Object,
            _userAdditionTask.Object,
            _getSaltTask.Object,
            _getSettingsTask.Object,
            _logger.Object);
    }

    [TestMethod]
    public async Task AddUserOperation_ExecuteSuccessfully()
    {
        const string login = "testLogin";
        const string pass = "password";

        _hashPasswordTask
            .Setup(d => d.HashAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(It.IsAny<string>());
        
        _userAdditionTask
            .Setup(d => d.AddAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(() => Task.CompletedTask);

        _getSaltTask
            .Setup(g => g.GetAsync(It.IsAny<int>(), It.IsAny<char>()))
            .ReturnsAsync(It.IsAny<string>());

        _getSettingsTask
            .Setup(g => g.GetAsync())
            .ReturnsAsync(() => new GetSettingsTaskResponse(It.IsAny<int>(), It.IsAny<char>()));

        await _addUserOperation.AddAsync(new AddUserOperationRequest(login, pass));

        _hashPasswordTask.Verify(h => h.HashAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        _userAdditionTask.Verify(h => h.AddAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        _getSaltTask.Verify(h => h.GetAsync(It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        _getSettingsTask.Verify(h => h.GetAsync(), Times.Once);
    }
}