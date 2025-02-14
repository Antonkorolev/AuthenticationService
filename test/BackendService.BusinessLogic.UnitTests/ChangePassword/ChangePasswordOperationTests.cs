using BackendService.BusinessLogic.Operations.ChangePassword;
using BackendService.BusinessLogic.Operations.ChangePassword.Models;
using BackendService.BusinessLogic.Operations.ChangePassword.Tasks.ChangePassword;
using BackendService.BusinessLogic.Tasks.GetHash;
using BackendService.BusinessLogic.Tasks.GetSalt;
using BackendService.BusinessLogic.Tasks.GetSettings;
using BackendService.BusinessLogic.Tasks.GetSettings.Models;
using BackendService.BusinessLogic.Tasks.HashPassword;
using BackendService.BusinessLogic.Tasks.ValidateUser;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BackendService.BusinessLogic.UnitTests.ChangePassword;

[TestClass]
public sealed class ChangePasswordOperationTests
{
    private Mock<IGetHashTask> _getHashTask = default!;
    private Mock<IHashPasswordTask> _hashPasswordTask = default!;
    private Mock<IValidateUserTask> _validateUserTask = default!;
    private Mock<IGetSaltTask> _getSaltTask = default!;
    private Mock<IGetSettingsTask> _getSettingsTask = default!;
    private Mock<IChangePasswordTask> _changePasswordTask = default!;
    private Mock<ILogger<ChangePasswordOperation>> _logger = default!;
    private ChangePasswordOperation _changePasswordOperation = default!;

    [TestInitialize]
    public void TestInitialize()
    {
        _getHashTask = new Mock<IGetHashTask>();
        _hashPasswordTask = new Mock<IHashPasswordTask>();
        _validateUserTask = new Mock<IValidateUserTask>();
        _getSaltTask = new Mock<IGetSaltTask>();
        _getSettingsTask = new Mock<IGetSettingsTask>();
        _getSettingsTask = new Mock<IGetSettingsTask>();
        _changePasswordTask = new Mock<IChangePasswordTask>();
        _logger = new Mock<ILogger<ChangePasswordOperation>>();

        _changePasswordOperation = new ChangePasswordOperation(
            _getHashTask.Object,
            _hashPasswordTask.Object,
            _validateUserTask.Object,
            _getSaltTask.Object,
            _getSettingsTask.Object,
            _changePasswordTask.Object,
            _logger.Object);
    }

    [TestMethod]
    public async Task ChangePasswordOperation_ExecuteSuccessfully()
    {
        const string login = "test";
        const string oldPassword = "OldPass";
        const string newPassword = "NewPass";
        const string salt = "salt";

        _hashPasswordTask
            .Setup(h => h.HashAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(It.IsAny<string>());

        _validateUserTask
            .Setup(v => v.ValidateAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        _getSaltTask
            .Setup(g => g.GetAsync(It.IsAny<int>(), It.IsAny<char>()))
            .ReturnsAsync(It.IsAny<string>());

        _getSettingsTask
            .Setup(g => g.GetAsync())
            .ReturnsAsync(new GetSettingsTaskResponse(It.IsAny<int>(), It.IsAny<char>()));

        await _changePasswordOperation
            .ChangeAsync(new ChangePasswordOperationRequest(login, oldPassword, newPassword))
            .ConfigureAwait(false);
        
        _hashPasswordTask.Verify(h => h.HashAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        _validateUserTask.Verify(v => v.ValidateAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        _getSaltTask.Verify(h => h.GetAsync(It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        _getSettingsTask.Verify(h => h.GetAsync(), Times.Once);
    }
}