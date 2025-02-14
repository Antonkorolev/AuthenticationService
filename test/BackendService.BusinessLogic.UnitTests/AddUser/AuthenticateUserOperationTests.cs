using BackendService.BusinessLogic.Operations.AuthenticateUser;
using BackendService.BusinessLogic.Operations.AuthenticateUser.Models;
using BackendService.BusinessLogic.Tasks.GetHash;
using BackendService.BusinessLogic.Tasks.ValidateUser;
using DatabaseContext.UserDb;
using DatabaseContext.UserDb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BackendService.BusinessLogic.UnitTests.AddUser;

[TestClass]
public sealed class AuthenticateUserOperationTests
{
    private IUserDbContext _userDbContext = default!;
    private Mock<IValidateUserTask> _validateUserTask = default!;
    private Mock<IGetHashTask> _getHashTask = default!;
    private Mock<ILogger<AuthenticateUserOperation>> _logger = default!;
    private IAuthenticateUserOperation _authenticateUserOperation = default!;

    [TestInitialize]
    public void TestInitialize()
    {
        _userDbContext = new UserDbContext(new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase($"UserDb")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options);

        _validateUserTask = new Mock<IValidateUserTask>();
        _getHashTask = new Mock<IGetHashTask>();
        _logger = new Mock<ILogger<AuthenticateUserOperation>>();

        _authenticateUserOperation = new AuthenticateUserOperation(
            _validateUserTask.Object,
            _getHashTask.Object,
            _logger.Object);
    }

    [TestMethod]
    public async Task AuthenticateUserOperation_ExecuteSuccessfully()
    {
        const string login = "testLogin";
        const string pass = "password";
        const string salt = "123";

        _validateUserTask
            .Setup(d => d.ValidateAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        _userDbContext.User.Add(new User { UserId = 1, Login = login, Password = pass,  Salt = salt});

        await _authenticateUserOperation.AuthenticateAsync(new AuthenticateUserOperationRequest(login, pass)).ConfigureAwait(false);

        _validateUserTask.Verify(a => a.ValidateAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }
}