using CoreFitnessClub.Domain.Exceptions;

namespace CoreFitnessClub.Domain.Entities;

public class TrainingClass : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;
    public string InstructorName { get; private set; } = string.Empty;
    public DateTime StartsAt { get; private set; }
    public DateTime EndsAt { get; private set; }
    public byte[]? RowVersion { get; set; }

    private TrainingClass()
    {
    }

    private TrainingClass(
        string name,
        string category,
        string instructorName,
        DateTime startsAt,
        DateTime endsAt)
    {
        UpdateDetails(name, category, instructorName);
        UpdateSchedule(startsAt, endsAt);
    }

    public static TrainingClass Create(
        string name,
        string category,
        string instructorName,
        DateTime startsAt,
        DateTime endsAt)
    {
        return new TrainingClass(name, category, instructorName, startsAt, endsAt);
    }

    public void UpdateDetails(string name, string category, string instructorName)
    {
        Name = name;
        Category = category;
        InstructorName = instructorName;
    }

    public void UpdateSchedule(DateTime startsAt, DateTime endsAt)
    {
        if (endsAt <= startsAt)
        {
            throw new InvalidTrainingClassTimeException();
        }

        StartsAt = startsAt;
        EndsAt = endsAt;
    }
}