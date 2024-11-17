using BackendService.BusinessLogic.Operations.AddUser;
using BackendService.BusinessLogic.Operations.AddUser.Tasks.HashPassword;
using BackendService.BusinessLogic.Operations.AddUser.Tasks.UserAddition;
using BackendService.BusinessLogic.Operations.AuthenticateUser;
using BackendService.BusinessLogic.Operations.AuthenticateUser.Tasks.ValidateUser;
using DatabaseContext.UserDb;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddUserDbContext(this IServiceCollection services, string name, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(name);

        services.AddDbContext<UserDbContext>((sp, options) =>
            {
                options.UseSqlServer(connectionString);
                options.UseLoggerFactory(sp.GetRequiredService<ILoggerFactory>());
            })
            .AddScoped<IUserDbContext>(x => x.GetRequiredService<UserDbContext>());

        return services;
    }

    public static IServiceCollection AddAuthenticateUserOperation(this IServiceCollection services)
    {
        services.AddTransient<IAuthenticateUserOperation, AuthenticateUserOperation>();
        services.AddTransient<IValidateUserTask, ValidateUserTask>();

        return services;
    }

    public static IServiceCollection AddAddUserOperation(this IServiceCollection services)
    {
        services.AddTransient<IAddUserOperation, AddUserOperation>();
        services.AddTransient<IHashPasswordTask, HashPasswordTask>();
        services.AddTransient<IUserAdditionTask, UserAdditionTask>();

        return services;
    }
}