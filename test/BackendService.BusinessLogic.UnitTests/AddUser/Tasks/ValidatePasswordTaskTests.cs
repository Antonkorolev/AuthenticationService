using BackendService.BusinessLogic.Exceptions;
using BackendService.BusinessLogic.Tasks.ValidatePassword;
using DatabaseContext.UserDb.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendService.BusinessLogic.UnitTests.AddUser.Tasks;

[TestClass]
public sealed class ValidatePasswordTaskTests
{
    
    private IValidatePasswordTask _validatePasswordTask = default!;

    [TestInitialize]
    public void TestInitialize()
    {
        _validatePasswordTask = new ValidatePasswordTask();
    }

    [TestMethod]
    public async Task ValidateUserTask_ReturnsTrue()
    {
        const string pass = "test1";
        const string hash = "$2b$12$RIVEHrguMT6i0j5hH0x4NuR4l3QWdwl8kvG7NaQDAxHxVibCMudGO";

        var result = await _validatePasswordTask.ValidateAsync(pass, hash);
        
        Assert.IsTrue(result);
    }
    
    [TestMethod]
    public async Task ValidateUserTask_ReturnsFalse()
    {
        const string pass = "test";
        const string hash = "$2b$12$RIVEHrguMT6i0j5hH0x4NuR4l3QWdwl8kvG7NaQDAxHxVibCMudGO";

        var result = await _validatePasswordTask.ValidateAsync(pass, hash);
        
        Assert.IsFalse(result);
    }
}