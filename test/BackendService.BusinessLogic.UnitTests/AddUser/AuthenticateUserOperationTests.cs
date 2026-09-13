using BackendService.AuthorizationService.Contracts;
using BackendService.AuthorizationService.Contracts.Request;
using BackendService.AuthorizationService.Contracts.Response;
using BackendService.BusinessLogic.Operations.AuthenticateUser;
using BackendService.BusinessLogic.Operations.AuthenticateUser.Models;
using BackendService.BusinessLogic.Tasks.CreateJwtToken;
using BackendService.BusinessLogic.Tasks.CreateJwtToken.Models;
using BackendService.BusinessLogic.Tasks.GetHash;
using BackendService.BusinessLogic.Tasks.GetHash.Models;
using BackendService.BusinessLogic.Tasks.ValidatePassword;
using DatabaseContext.UserDb;
using DatabaseContext.UserDb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Permission = BackendService.AuthorizationService.Contracts.Response.Permission;
using RoleInfo = BackendService.AuthorizationService.Contracts.Request.RoleInfo;

namespace BackendService.BusinessLogic.UnitTests.AddUser;

[TestClass]
public sealed class AuthenticateUserOperationTests
{
    private IUserDbContext _userDbContext = default!;
    private Mock<IValidatePasswordTask> _validateUserTask = default!;
    private Mock<IGetHashTask> _getHashTask = default!;
    private Mock<IAuthorizationServiceClient> _authorizationServiceClient = default!;
    private Mock<ICreateJwtTokenTask> _createJwtTokenTask = default!;
    private Mock<ILogger<AuthenticateUserOperation>> _logger = default!;
    private IAuthenticateUserOperation _authenticateUserOperation = default!;

    [TestInitialize]
    public void TestInitialize()
    {
        _userDbContext = new UserDbContext(new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase($"UserDb")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options);

        _validateUserTask = new Mock<IValidatePasswordTask>();
        _getHashTask = new Mock<IGetHashTask>();
        _authorizationServiceClient = new Mock<IAuthorizationServiceClient>();
        _createJwtTokenTask = new Mock<ICreateJwtTokenTask>();
        _logger = new Mock<ILogger<AuthenticateUserOperation>>();

        _authenticateUserOperation = new AuthenticateUserOperation(
            _validateUserTask.Object,
            _getHashTask.Object,
            _authorizationServiceClient.Object,
            _createJwtTokenTask.Object,
            _logger.Object);
    }

    [TestMethod]
    public async Task AuthenticateUserOperation_ExecuteSuccessfully()
    {
        const string login = "testLogin";
        const string pass = "password";
        const string salt = "123";
        
        var rolePermission = new RolePermission[] { new("Role", new[] { new Permission("TestPerm", null) }) };
        
        _getHashTask
            .Setup(g => g.GetAsync(login))
            .ReturnsAsync(new GetHashTaskResponse(1, pass));
        
        _validateUserTask
            .Setup(d => d.ValidateAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        _userDbContext.User.Add(new User { UserId = 1, Login = login, Password = pass, Salt = salt });

        _authorizationServiceClient
            .Setup(a => a.GetPermissionsAsync(It.Is<GetPermissionsRequest>(x => x.Login == login)))
            .ReturnsAsync(new GetPermissionsResponse(rolePermission));
            
        _createJwtTokenTask
            .Setup(c => c.CreateAsync(It.IsAny<CreateJwtTokenTaskRequest>()))
            .ReturnsAsync("testToken");
        
        await _authenticateUserOperation.AuthenticateAsync(new AuthenticateUserOperationRequest(login, pass)).ConfigureAwait(false);

        _validateUserTask.Verify(a => a.ValidateAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        _createJwtTokenTask.Verify(a => a.CreateAsync(It.IsAny<CreateJwtTokenTaskRequest>()), Times.Once);
    }
}