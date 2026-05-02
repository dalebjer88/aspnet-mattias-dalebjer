using CoreFitnessClub.Domain.Entities;
using CoreFitnessClub.Domain.Exceptions;
using Xunit;

namespace CoreFitnessClub.Tests.Unit.Domain;

public class TrainingClassTests
{
    [Fact]
    public void Create_WithEndTimeBeforeStartTime_ThrowsInvalidTrainingClassTimeException()
    {
        var startsAt = new DateTime(2026, 5, 3, 10, 0, 0);
        var endsAt = new DateTime(2026, 5, 3, 9, 0, 0);

        var action = () => TrainingClass.Create(
            "Morning Yoga",
            "Yoga",
            "Anna Berg",
            startsAt,
            endsAt);

        Assert.Throws<InvalidTrainingClassTimeException>(action);
    }

    [Fact]
    public void Create_WithEndTimeSameAsStartTime_ThrowsInvalidTrainingClassTimeException()
    {
        var startsAt = new DateTime(2026, 5, 3, 10, 0, 0);
        var endsAt = new DateTime(2026, 5, 3, 10, 0, 0);

        var action = () => TrainingClass.Create(
            "Morning Yoga",
            "Yoga",
            "Anna Berg",
            startsAt,
            endsAt);

        Assert.Throws<InvalidTrainingClassTimeException>(action);
    }

    [Fact]
    public void Create_WithValidTimes_CreatesTrainingClass()
    {
        var startsAt = new DateTime(2026, 5, 3, 10, 0, 0);
        var endsAt = new DateTime(2026, 5, 3, 11, 0, 0);

        var trainingClass = TrainingClass.Create(
            "Morning Yoga",
            "Yoga",
            "Anna Berg",
            startsAt,
            endsAt);

        Assert.Equal("Morning Yoga", trainingClass.Name);
        Assert.Equal("Yoga", trainingClass.Category);
        Assert.Equal("Anna Berg", trainingClass.InstructorName);
        Assert.Equal(startsAt, trainingClass.StartsAt);
        Assert.Equal(endsAt, trainingClass.EndsAt);
    }

    [Fact]
    public void UpdateSchedule_WithEndTimeBeforeStartTime_ThrowsInvalidTrainingClassTimeException()
    {
        var trainingClass = TrainingClass.Create(
            "Morning Yoga",
            "Yoga",
            "Anna Berg",
            new DateTime(2026, 5, 3, 10, 0, 0),
            new DateTime(2026, 5, 3, 11, 0, 0));

        var newStartsAt = new DateTime(2026, 5, 4, 12, 0, 0);
        var newEndsAt = new DateTime(2026, 5, 4, 11, 0, 0);

        var action = () => trainingClass.UpdateSchedule(newStartsAt, newEndsAt);

        Assert.Throws<InvalidTrainingClassTimeException>(action);
    }

    [Fact]
    public void UpdateSchedule_WithValidTimes_UpdatesSchedule()
    {
        var trainingClass = TrainingClass.Create(
            "Morning Yoga",
            "Yoga",
            "Anna Berg",
            new DateTime(2026, 5, 3, 10, 0, 0),
            new DateTime(2026, 5, 3, 11, 0, 0));

        var newStartsAt = new DateTime(2026, 5, 4, 12, 0, 0);
        var newEndsAt = new DateTime(2026, 5, 4, 13, 0, 0);

        trainingClass.UpdateSchedule(newStartsAt, newEndsAt);

        Assert.Equal(newStartsAt, trainingClass.StartsAt);
        Assert.Equal(newEndsAt, trainingClass.EndsAt);
    }
}