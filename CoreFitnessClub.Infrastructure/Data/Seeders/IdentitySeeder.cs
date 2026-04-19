using CoreFitnessClub.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace CoreFitnessClub.Infrastructure.Data.Seeders;

public static class IdentitySeeder
{
    public static async Task SeedAdminAsync(
        RoleManager<IdentityRole> roleManager,
        UserManager<AppUser> userManager)
    {
        const string adminRole = "Admin";
        const string memberRole = "Member";
        const string adminEmail = "admin@corefitnessclub.com";
        const string adminPassword = "Admin123";

        if (!await roleManager.RoleExistsAsync(adminRole))
        {
            await roleManager.CreateAsync(new IdentityRole(adminRole));
        }

        if (!await roleManager.RoleExistsAsync(memberRole))
        {
            await roleManager.CreateAsync(new IdentityRole(memberRole));
        }

        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser is null)
        {
            adminUser = new AppUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(adminUser, adminPassword);

            if (!createResult.Succeeded)
            {
                return;
            }
        }

        if (!adminUser.EmailConfirmed)
        {
            adminUser.EmailConfirmed = true;
            await userManager.UpdateAsync(adminUser);
        }

        if (!await userManager.IsInRoleAsync(adminUser, adminRole))
        {
            await userManager.AddToRoleAsync(adminUser, adminRole);
        }
    }
}