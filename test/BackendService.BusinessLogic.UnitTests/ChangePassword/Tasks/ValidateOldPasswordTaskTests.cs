using BackendService.BusinessLogic.Exceptions;
using BackendService.BusinessLogic.Operations.ChangePassword.Tasks.ValidateOldPassword;
using BackendService.BusinessLogic.Operations.ChangePassword.Tasks.ValidateOldPassword.Models;
using BackendService.BusinessLogic.Tasks.GetHash;
using BackendService.BusinessLogic.Tasks.ValidatePassword;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BackendService.BusinessLogic.UnitTests.ChangePassword.Tasks;

[TestClass]
public sealed class ValidateOldPasswordTaskTests
{
    private Mock<IGetHashTask> _getHashTask = default!;
    private Mock<IValidatePasswordTask> _validatePasswordTask = default!;
    private ValidateOldPasswordTask _validateOldPasswordTask = default!; 

    [TestInitialize]
    public void TestInitialize()
    {
        _getHashTask = new Mock<IGetHashTask>();
        _validatePasswordTask = new Mock<IValidatePasswordTask>();

        _validateOldPasswordTask = new ValidateOldPasswordTask(_getHashTask.Object, _validatePasswordTask.Object);
    }

    [TestMethod]
    public async Task ValidateOldPasswordTask_UserVerified()
    {
        const string login = "Test";
        const string password = "test";
        const string hash = "$2b$12$emPfIW8rmhnJuyLZt6FLPe";

        _getHashTask
            .Setup(g => g.GetAsync(login))
            .ReturnsAsync(hash);

        _validatePasswordTask
            .Setup(v => v.ValidateAsync(password, hash))
            .ReturnsAsync(true);

        await _validateOldPasswordTask.ValidateAsync(new ValidateOldPasswordTaskRequest(login, password));

        _getHashTask.Verify(g => g.GetAsync(login), Times.Once);
        _validatePasswordTask.Verify(v => v.ValidateAsync(password, hash), Times.Once);
    }
    
    [TestMethod]
    public async Task ValidateOldPasswordTask_UserNotVerified()
    {
        _getHashTask
            .Setup(g => g.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(It.IsAny<string>());

        _validatePasswordTask
            .Setup(v => v.ValidateAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(false);

        var exception = await Assert.ThrowsExceptionAsync<UserVerifiedException>(() => _validateOldPasswordTask.ValidateAsync(new ValidateOldPasswordTaskRequest(It.IsAny<string>(), It.IsAny<string>())));

        Assert.AreEqual($"User not verified", exception.Message);
    }
}