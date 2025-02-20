using BackendService.BusinessLogic.Operations.ChangePassword.Tasks.ChangePassword;
using BackendService.BusinessLogic.Operations.ChangePassword.Tasks.ChangePassword.Models;
using BackendService.BusinessLogic.Tasks.GetSalt;
using BackendService.BusinessLogic.Tasks.GetSettings;
using BackendService.BusinessLogic.Tasks.GetSettings.Models;
using BackendService.BusinessLogic.Tasks.HashPassword;
using DatabaseContext.UserDb;
using DatabaseContext.UserDb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BackendService.BusinessLogic.UnitTests.ChangePassword.Tasks;

[TestClass]
public sealed class ChangePasswordTaskTests
{
    private UserDbContext _dbContext = default!;
    private Mock<IGetSettingsTask> _getSettingsTask = default!;
    private Mock<IGetSaltTask> _getSaltTask = default!;
    private Mock<IHashPasswordTask> _hashPasswordTask = default!;
    private ChangePasswordTask _changePasswordTask = default!;

    [TestInitialize]
    public void TestInitialize()
    {
        _dbContext = new UserDbContext(new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase("UserDb")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options);

        _getSettingsTask = new Mock<IGetSettingsTask>();
        _getSaltTask = new Mock<IGetSaltTask>();
        _hashPasswordTask = new Mock<IHashPasswordTask>();

        _changePasswordTask = new ChangePasswordTask(_dbContext, _getSettingsTask.Object, _getSaltTask.Object, _hashPasswordTask.Object);
    }

    [TestMethod]
    public async Task ChangePasswordTask_SuccessfullyChanged()
    {
        const int userId = 1;
        const string login = "test";

        const string newPassword = "test";
        const string newHash = "$2b$12$emPfIW8rmhnJuyLZt6FLPeXJ4wXwNaT5LrJ18LqYfd9f5cI5zMvOO"; // test
        const string newSalt = "$2b$12$emPfIW8rmhnJuyLZt6FLPe";
        
        const string oldHash = "$2b$12$RIVEHrguMT6i0j5hH0x4NuR4l3QWdwl8kvG7NaQDAxHxVibCMudGO"; // test1
        const string oldSalt = "$2b$12$RIVEHrguMT6i0j5hH0x4Nu";

        const int workFactor = 12;
        const char bcryptMinorRevision = 'b';

        await _dbContext.Settings.AddRangeAsync(
            new Settings { SettingId = 1, Key = "WorkFactor", Value = "12" }, 
            new Settings { SettingId = 2, Key = "BcryptMinorRevision", Value = "b"});

        await _dbContext.User.AddAsync(new User { UserId = userId, Login = login, Password = oldHash, Salt = oldSalt });
        await _dbContext.SaveChangesAsync(CancellationToken.None);

        _getSettingsTask
            .Setup(g => g.GetAsync())
            .ReturnsAsync(new GetSettingsTaskResponse(workFactor, 'b'));

        _getSaltTask
            .Setup(g => g.GetAsync(workFactor, bcryptMinorRevision))
            .ReturnsAsync(newSalt);

        _hashPasswordTask
            .Setup(h => h.HashAsync(newPassword, newSalt))
            .ReturnsAsync(newHash);

        await _changePasswordTask.ChangeAsync(new ChangePasswordRequest(login, newPassword));

        var user = await _dbContext.User.FirstOrDefaultAsync(u => u.UserId == userId);
        
        Assert.IsNotNull(user);
        Assert.AreEqual(login, user.Login);
        Assert.AreEqual(newHash, user.Password);
        Assert.AreEqual(newSalt, user.Salt);
    }
}