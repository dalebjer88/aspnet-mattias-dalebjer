using CoreFitnessClub.Application.Features.Account;
using Microsoft.Extensions.DependencyInjection;
using CoreFitnessClub.Application.Features.Memberships;

namespace CoreFitnessClub.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IReadMembershipService, ReadMembershipService>();
        services.AddScoped<IMembershipService, MembershipService>();
        return services;
    }
}