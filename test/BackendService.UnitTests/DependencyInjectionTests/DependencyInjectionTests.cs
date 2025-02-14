using System.Text;
using BackendService.BusinessLogic.Operations.AddUser;
using BackendService.BusinessLogic.Operations.AuthenticateUser;
using BackendService.BusinessLogic.Operations.ChangePassword;
using BackendService.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendService.UnitTests.DependencyInjectionTests;

[TestClass]
public sealed class DependencyInjectionTests
{
    private ServiceProvider _serviceProvider = default!;

    private const string AppSettings =
        """
            {
             "ConnectionStrings": {
                "UserDb": "*"
                }
           }
        """;
    
    [TestInitialize]
    public void TestInitialize()
    {
        var byteArray = Encoding.ASCII.GetBytes(AppSettings);
        var configurationStream = new MemoryStream(byteArray);

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<ILoggerFactory>(_ => NullLoggerFactory.Instance);
        serviceCollection.AddLogging();

        var configuration = new ConfigurationBuilder()
            .AddJsonStream(configurationStream)
            .Build();

        serviceCollection.AddUserDbContext("UserDb", configuration);
        serviceCollection.AddAuthenticateUserOperation();
        serviceCollection.AddAddUserOperation();
        serviceCollection.AddChangePasswordOperation();
        serviceCollection.AddCommonTasks();

        _serviceProvider = serviceCollection.BuildServiceProvider();
    }
    
    [TestMethod]
    public void AddAuthenticateUserOperation_ConfigureCorrectly()
    {
        var service = _serviceProvider.GetService<IAuthenticateUserOperation>();

        Assert.IsNotNull(service);
    }
    
    [TestMethod]
    public void AddAddUserOperation_ConfigureCorrectly()
    {
        var service = _serviceProvider.GetService<IAddUserOperation>();

        Assert.IsNotNull(service);
    }
    
    [TestMethod]
    public void ChangePasswordOperation_ConfigureCorrectly()
    {
        var service = _serviceProvider.GetService<IChangePasswordOperation>();

        Assert.IsNotNull(service);
    }
}