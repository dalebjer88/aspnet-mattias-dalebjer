using CoreFitnessClub.Domain.Entities;

namespace CoreFitnessClub.Infrastructure.Data.Seeders;

public static class TrainingClassSeeder
{
    public static async Task SeedAsync(CoreFitnessClubDbContext dbContext)
    {
        if (dbContext.TrainingClasses.Any())
        {
            return;
        }

        var trainingClasses = new List<TrainingClass>
        {
            TrainingClass.Create(
                "Morning Yoga",
                "Yoga",
                "Anna Berg",
                new DateTime(2026, 4, 13, 9, 0, 0),
                new DateTime(2026, 4, 13, 10, 0, 0)
            ),
            TrainingClass.Create(
                "Strength Circuit",
                "Strength",
                "Erik Nilsson",
                new DateTime(2026, 4, 13, 17, 30, 0),
                new DateTime(2026, 4, 13, 18, 30, 0)
            ),
            TrainingClass.Create(
                "Evening Cardio Blast",
                "Cardio",
                "Sofia Lind",
                new DateTime(2026, 4, 14, 18, 0, 0),
                new DateTime(2026, 4, 14, 19, 0, 0)
            )
        };

        dbContext.TrainingClasses.AddRange(trainingClasses);
        await dbContext.SaveChangesAsync();
    }
}