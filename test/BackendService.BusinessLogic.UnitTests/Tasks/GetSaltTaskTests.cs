using BackendService.BusinessLogic.Tasks.GetSalt;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendService.BusinessLogic.UnitTests.Tasks;

[TestClass]
public sealed class GetSaltTaskTests
{
    private IGetSaltTask _getSaltTask = default!;

    [TestInitialize]
    public void TestInitialize()
    {
        _getSaltTask = new GetSaltTask();
    }

    [TestMethod]
    public async Task ValidateUserTask_ReturnsTrue()
    {
        const int workFactor = 4;
        const char bcryptMinorRevision = 'b';

        var result = await _getSaltTask.GetAsync(workFactor, bcryptMinorRevision);

        Assert.IsNotNull(result);
    }
}