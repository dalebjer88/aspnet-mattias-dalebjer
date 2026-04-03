using CoreFitnessClub.Application.Features.Account;
using Microsoft.Extensions.DependencyInjection;

namespace CoreFitnessClub.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();

        return services;
    }
}