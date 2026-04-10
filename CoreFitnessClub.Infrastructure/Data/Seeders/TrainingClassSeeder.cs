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
            new()
            {
                Name = "Morning Yoga",
                Category = "Yoga",
                InstructorName = "Anna Berg",
                StartsAt = new DateTime(2026, 4, 13, 9, 0, 0),
                EndsAt = new DateTime(2026, 4, 13, 10, 0, 0)
            },
            new()
            {
                Name = "Strength Circuit",
                Category = "Strength",
                InstructorName = "Erik Nilsson",
                StartsAt = new DateTime(2026, 4, 13, 17, 30, 0),
                EndsAt = new DateTime(2026, 4, 13, 18, 30, 0)
            },
            new()
            {
                Name = "Evening Cardio Blast",
                Category = "Cardio",
                InstructorName = "Sofia Lind",
                StartsAt = new DateTime(2026, 4, 14, 18, 0, 0),
                EndsAt = new DateTime(2026, 4, 14, 19, 0, 0)
            }
        };

        dbContext.TrainingClasses.AddRange(trainingClasses);
        await dbContext.SaveChangesAsync();
    }
}