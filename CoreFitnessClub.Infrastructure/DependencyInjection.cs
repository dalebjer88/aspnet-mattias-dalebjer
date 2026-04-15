using CoreFitnessClub.Application.Abstractions;
using CoreFitnessClub.Infrastructure.Data;
using CoreFitnessClub.Infrastructure.Data.Repositories;
using CoreFitnessClub.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoreFitnessClub.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            services.AddDbContext<CoreFitnessClubDbContext>(options =>
                options.UseInMemoryDatabase("CoreFitnessClubDb"));
        }
        else
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

            services.AddDbContext<CoreFitnessClubDbContext>(options =>
                options.UseSqlServer(connectionString));
        }

        services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = true;
        })
            .AddEntityFrameworkStores<CoreFitnessClubDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication()
            .AddGoogle(options =>
            {
                options.ClientId = configuration["Authentication:Google:ClientId"]
                    ?? throw new InvalidOperationException("Google ClientId is not configured.");

                options.ClientSecret = configuration["Authentication:Google:ClientSecret"]
                    ?? throw new InvalidOperationException("Google ClientSecret is not configured.");
            });

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Auth/SignIn";
            options.AccessDeniedPath = "/Auth/AccessDenied";
        });

        services.AddHttpContextAccessor();

        services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IIdentityUserService, IdentityUserService>();
        services.AddScoped<IMembershipRepository, MembershipRepository>();
        services.AddScoped<ITrainingClassRepository, TrainingClassRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();

        return services;
    }
}