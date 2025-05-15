using Microsoft.AspNetCore.Identity;
using WalletApi.Application.Interfaces.Repositories;
using WalletApi.Application.Interfaces.Services;
using WalletApi.Application.Services;
using WalletApi.Domain.Entities;
using WalletApi.Infrastructure.Repository;

namespace WalletApi.Infrastructure.IOC;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Application Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IWalletService, WalletService>();
        services.AddTransient<IEmailSender<AppUser>, EmailSenderService>();
        services.AddScoped<ITransferService, TransferService>();
        services.AddScoped<IUserService, UserService>();

        // Repository
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITransferRepository, TransferRepository>();
        
        return services;
    }
}

