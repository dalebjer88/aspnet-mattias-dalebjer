using CoreFitnessClub.Application.Features.Account;
using CoreFitnessClub.Application.Features.Bookings;
using CoreFitnessClub.Application.Features.Classes;
using CoreFitnessClub.Application.Features.Memberships;
using Microsoft.Extensions.DependencyInjection;

namespace CoreFitnessClub.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IReadMembershipService, ReadMembershipService>();
        services.AddScoped<IMembershipService, MembershipService>();
        services.AddScoped<IReadTrainingClassService, ReadTrainingClassService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IManageTrainingClassService, ManageTrainingClassService>();
        return services;
    }
}