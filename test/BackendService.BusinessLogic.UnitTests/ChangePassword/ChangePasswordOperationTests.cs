using BackendService.BusinessLogic.Operations.ChangePassword;
using BackendService.BusinessLogic.Operations.ChangePassword.Models;
using BackendService.BusinessLogic.Operations.ChangePassword.Tasks.ChangePassword;
using BackendService.BusinessLogic.Operations.ChangePassword.Tasks.ValidateOldPassword;
using BackendService.BusinessLogic.Tasks.GetHash;
using BackendService.BusinessLogic.Tasks.GetSalt;
using BackendService.BusinessLogic.Tasks.GetSettings;
using BackendService.BusinessLogic.Tasks.GetSettings.Models;
using BackendService.BusinessLogic.Tasks.HashPassword;
using BackendService.BusinessLogic.Tasks.ValidatePassword;
using DatabaseContext.UserDb;
using DatabaseContext.UserDb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BackendService.BusinessLogic.UnitTests.ChangePassword;

[TestClass]
public sealed class ChangePasswordOperationTests
{
    private UserDbContext _userDbContext = default!;

    private Mock<IValidatePasswordTask> _validatePasswordTask = default!;
    private Mock<IGetHashTask> _getHashTask = default!;
    private IValidateOldPasswordTask _validateOldPasswordTask = default!;


    private Mock<IGetSettingsTask> _getSettingsTask = default!;
    private Mock<IGetSaltTask> _getSaltTask = default!;
    private Mock<IHashPasswordTask> _hashPasswordTask = default!;
    private IChangePasswordTask _changePasswordTask = default!;


    private Mock<ILogger<ChangePasswordOperation>> _logger = default!;
    private ChangePasswordOperation _changePasswordOperation = default!;

    [TestInitialize]
    public void TestInitialize()
    {
        _userDbContext = new UserDbContext(new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase("UserDb")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options);

        _getHashTask = new Mock<IGetHashTask>();
        _validatePasswordTask = new Mock<IValidatePasswordTask>();

        _validateOldPasswordTask = new ValidateOldPasswordTask(_getHashTask.Object, _validatePasswordTask.Object);

        _getSettingsTask = new Mock<IGetSettingsTask>();
        _getSaltTask = new Mock<IGetSaltTask>();
        _hashPasswordTask = new Mock<IHashPasswordTask>();

        _changePasswordTask = new ChangePasswordTask(_userDbContext, _getSettingsTask.Object, _getSaltTask.Object, _hashPasswordTask.Object);
        _logger = new Mock<ILogger<ChangePasswordOperation>>();

        _changePasswordOperation = new ChangePasswordOperation(
            _validateOldPasswordTask,
            _changePasswordTask,
            _logger.Object);
    }

    [TestMethod]
    public async Task ChangePasswordOperation_ExecuteSuccessfully()
    {
        const string login = "test";
        const string oldPassword = "OldPass";
        const string newPassword = "NewPass";
        const string salt = "salt";

        await _userDbContext.AddAsync(new User { Login = login, Password = oldPassword, Salt = salt }).ConfigureAwait(false);
        await _userDbContext.SaveChangesAsync(CancellationToken.None).ConfigureAwait(false);

        _hashPasswordTask
            .Setup(h => h.HashAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(It.IsAny<string>());

        _validatePasswordTask
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
        _validatePasswordTask.Verify(v => v.ValidateAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        _getSaltTask.Verify(h => h.GetAsync(It.IsAny<int>(), It.IsAny<char>()), Times.Once);
        _getSettingsTask.Verify(h => h.GetAsync(), Times.Once);
    }
}