using CoreFitnessClub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreFitnessClub.Infrastructure.Data.Seeders;

public static class TrainingClassSeeder
{
    public static async Task SeedAsync(CoreFitnessClubDbContext dbContext)
    {
        var today = DateTime.Today;

        var hasFutureTrainingClasses = await dbContext.TrainingClasses
            .AnyAsync(x => x.StartsAt > DateTime.Now);

        if (hasFutureTrainingClasses)
        {
            return;
        }

        var trainingClasses = new List<TrainingClass>
        {
            TrainingClass.Create(
                "Morning Yoga",
                "Yoga",
                "Anna Berg",
                today.AddDays(1).AddHours(9),
                today.AddDays(1).AddHours(10)
            ),
            TrainingClass.Create(
                "Strength Circuit",
                "Strength",
                "Erik Nilsson",
                today.AddDays(1).AddHours(17).AddMinutes(30),
                today.AddDays(1).AddHours(18).AddMinutes(30)
            ),
            TrainingClass.Create(
                "Evening Cardio Blast",
                "Cardio",
                "Sofia Lind",
                today.AddDays(2).AddHours(18),
                today.AddDays(2).AddHours(19)
            ),
            TrainingClass.Create(
                "HIIT Fundamentals",
                "HIIT",
                "Marcus Lindgren",
                today.AddDays(3).AddHours(12),
                today.AddDays(3).AddHours(12).AddMinutes(45)
            ),
            TrainingClass.Create(
                "Mobility Flow",
                "Mobility",
                "Sara Holm",
                today.AddDays(4).AddHours(16),
                today.AddDays(4).AddHours(17)
            )
        };

        await dbContext.TrainingClasses.AddRangeAsync(trainingClasses);
        await dbContext.SaveChangesAsync();
    }
}