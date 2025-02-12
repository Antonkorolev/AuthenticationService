using BackendService.BusinessLogic.Exceptions;
using BackendService.BusinessLogic.Tasks.ValidateUser;
using DatabaseContext.UserDb.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendService.BusinessLogic.UnitTests.AddUser.Tasks;

[TestClass]
public sealed class ValidateUserTaskTests
{
    
    private IValidateUserTask _validateUserTask = default!;

    [TestInitialize]
    public void TestInitialize()
    {
        _validateUserTask = new ValidateUserTask();
    }

    [TestMethod]
    public async Task ValidateUserTask_ReturnsTrue()
    {
        const string login = "testLogin";
        const string pass = "test1";
        const string expectedPass = "$2b$12$RIVEHrguMT6i0j5hH0x4NuR4l3QWdwl8kvG7NaQDAxHxVibCMudGO";
        const string salt = "$2b$12$RIVEHrguMT6i0j5hH0x4Nu";

        var result = await _validateUserTask.ValidateAsync(new User { UserId = 1, Login = login, Password = expectedPass, Salt = salt }, login, pass);
        
        Assert.IsTrue(result);
    }
    
    [TestMethod]
    public async Task ValidateUserTask_ReturnsFalse()
    {
        const string login = "testLogin";
        const string pass = "test";
        const string expectedPass = "$2b$12$RIVEHrguMT6i0j5hH0x4NuR4l3QWdwl8kvG7NaQDAxHxVibCMudGO";
        const string salt = "$2b$12$RIVEHrguMT6i0j5hH0x4Nu";

        var result = await _validateUserTask.ValidateAsync(new User { UserId = 1, Login = login, Password = expectedPass, Salt = salt }, login, pass);
        
        Assert.IsFalse(result);
    }
    
    [TestMethod]
    public async Task ValidateUserTask_UserIsNull_ThrowsException()
    {
        const string login = "testLogin";
        const string pass = "test";
        
        var exception = await Assert.ThrowsExceptionAsync<UserNotFoundException>(() => _validateUserTask.ValidateAsync(null, login, pass));
        
        Assert.AreEqual($"User not found by login: {login}", exception.Message);
    }
}