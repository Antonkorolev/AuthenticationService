using BackendService.BusinessLogic.Exceptions;
using BackendService.BusinessLogic.Tasks.GetSettings;
using DatabaseContext.UserDb;
using DatabaseContext.UserDb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BackendService.BusinessLogic.UnitTests.AuthenticateUser.Tasks;

[TestClass]
public sealed class GetSettingsTaskTests
{
    public TestContext TestContext { get; set; } = default!;
    
    private IUserDbContext _userDbContext = default!;
    private Mock<ILogger<GetSettingsTask>> _logger = default!;
    private IGetSettingsTask _getSettingsTask = default!;

    [TestInitialize]
    public void TestInitialize()
    {
        _userDbContext = new UserDbContext(new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase($"UserDb_{TestContext.TestName}")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options);

        _logger = new Mock<ILogger<GetSettingsTask>>();

        _getSettingsTask = new GetSettingsTask(_userDbContext, _logger.Object);
    }

    [TestMethod]
    public async Task GetSettingsTask_SuccessfulResponse()
    {
        const string key1 = "WorkFactor";
        const string key2 = "BcryptMinorRevision";

        const string value1 = "12";
        const string value2 = "b";

        await _userDbContext.Settings.AddRangeAsync(
            new Settings { SettingId = 1, Key = key1, Value = value1 },
            new Settings { SettingId = 2, Key = key2, Value = value2 });
        await _userDbContext.SaveChangesAsync(CancellationToken.None);

        var getSettingsTaskResponse = await _getSettingsTask.GetAsync();

        Assert.AreEqual(int.Parse(value1), getSettingsTaskResponse.WorkFactor);
        Assert.AreEqual(char.Parse(value2), getSettingsTaskResponse.BcryptMinorRevision);
    }
    
    [TestMethod]
    public async Task GetSettingsTask_WorkFactorNotFound_ThrowsException()
    {
        const string key = "BcryptMinorRevision";
        const string value = "b";
        
        await _userDbContext.Settings.AddRangeAsync(new Settings { SettingId = 1, Key = key, Value = value });
        await _userDbContext.SaveChangesAsync(CancellationToken.None);
        
        var exception = await Assert.ThrowsExceptionAsync<SettingNotFoundException>(() => _getSettingsTask.GetAsync());
        
        Assert.AreEqual($"Setting by key = 'WorkFactor' not found", exception.Message);
    }
    
    [TestMethod]
    public async Task GetSettingsTask_BcryptMinorRevisionNotFound_ThrowsException()
    {
        const string key = "WorkFactor";
        const string value = "12";
        
        await _userDbContext.Settings.AddRangeAsync(new Settings { SettingId = 1, Key = key, Value = value });
        await _userDbContext.SaveChangesAsync(CancellationToken.None);
        
        var exception = await Assert.ThrowsExceptionAsync<SettingNotFoundException>(() => _getSettingsTask.GetAsync());
        
        Assert.AreEqual($"Setting by key = 'BcryptMinorRevision' not found", exception.Message);
    }
}