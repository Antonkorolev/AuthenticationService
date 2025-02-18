using BackendService.BusinessLogic.Operations.AddUser.Tasks.UserAddition;
using DatabaseContext.UserDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendService.BusinessLogic.UnitTests.AuthenticateUser.Tasks;

[TestClass]
public sealed class UserAdditionTaskTests
{
    private UserDbContext _userDbContext = default!;
    private IUserAdditionTask _userAdditionTask = default!;

    [TestInitialize]
    public void TestInitialize()
    {
        _userDbContext = new UserDbContext(new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase("UserDb")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options);

        _userAdditionTask = new UserAdditionTask(_userDbContext);
    }


    [TestMethod]
    public async Task UserAdditionTask_SuccessfulResponse()
    {
        const string login = "testLogin";
        const string pass = "password";
        const string salt = "123";
        
        await _userAdditionTask.AddAsync(login, pass, salt).ConfigureAwait(false);

        var user = _userDbContext.User.FirstOrDefault(u => u.Login == login);
        
        Assert.IsNotNull(user);
        Assert.AreEqual(pass, user.Password);
        Assert.AreEqual(salt, user.Salt);
    }
}