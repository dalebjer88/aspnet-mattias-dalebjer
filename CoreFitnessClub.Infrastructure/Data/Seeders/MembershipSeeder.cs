using CoreFitnessClub.Domain.Entities;

namespace CoreFitnessClub.Infrastructure.Data.Seeders;

public static class MembershipSeeder
{
    public static async Task SeedAsync(CoreFitnessClubDbContext dbContext)
    {
        if (dbContext.MembershipPlans.Any())
        {
            return;
        }

        var standardPlan = new MembershipPlan
        {
            Name = "Standard Membership",
            PricePerMonth = 495.00m,
            ClassesPerMonth = 20,
            TrialWeeks = 1,
            Features =
            [
                new MembershipPlanFeature { Text = "Standard Locker" },
                new MembershipPlanFeature { Text = "High-energy group fitness classes" },
                new MembershipPlanFeature { Text = "Motivating & supportive environment" }
            ]
        };

        var premiumPlan = new MembershipPlan
        {
            Name = "Premium Membership",
            PricePerMonth = 795.00m,
            ClassesPerMonth = 40,
            TrialWeeks = 1,
            Features =
            [
                new MembershipPlanFeature { Text = "Priority Support & Premium Locker" },
                new MembershipPlanFeature { Text = "High-energy group fitness classes" },
                new MembershipPlanFeature { Text = "Motivating & supportive environment" }
            ]
        };

        dbContext.MembershipPlans.AddRange(standardPlan, premiumPlan);
        await dbContext.SaveChangesAsync();
    }
}