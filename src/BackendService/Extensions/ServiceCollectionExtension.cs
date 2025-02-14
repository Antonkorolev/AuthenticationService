using BackendService.BusinessLogic.Operations.AddUser;
using BackendService.BusinessLogic.Operations.AddUser.Tasks.UserAddition;
using BackendService.BusinessLogic.Operations.AuthenticateUser;
using BackendService.BusinessLogic.Operations.ChangePassword;
using BackendService.BusinessLogic.Operations.ChangePassword.Tasks.ChangePassword;
using BackendService.BusinessLogic.Tasks.GetHash;
using BackendService.BusinessLogic.Tasks.GetSalt;
using BackendService.BusinessLogic.Tasks.GetSettings;
using BackendService.BusinessLogic.Tasks.HashPassword;
using BackendService.BusinessLogic.Tasks.ValidateUser;
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
        services.AddTransient<IUserAdditionTask, UserAdditionTask>();

        return services;
    }

    public static IServiceCollection AddChangePasswordOperation(this IServiceCollection services)
    {
        services.AddTransient<IChangePasswordOperation, ChangePasswordOperation>();
        services.AddTransient<IChangePasswordTask, ChangePasswordTask>();

        return services;
    }

    public static IServiceCollection AddCommonTasks(this IServiceCollection services)
    {
        services.AddTransient<IHashPasswordTask, HashPasswordTask>();
        services.AddTransient<IGetSettingsTask, GetSettingsTask>();
        services.AddTransient<IGetSaltTask, GetSaltTask>();
        services.AddTransient<IValidateUserTask, ValidateUserTask>();
        services.AddTransient<IGetHashTask, GetHashTask>();

        return services;
    }
}