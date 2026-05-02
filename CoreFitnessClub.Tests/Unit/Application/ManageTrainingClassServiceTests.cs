using CoreFitnessClub.Application.Abstractions;
using CoreFitnessClub.Application.Features.Classes;
using CoreFitnessClub.Domain.Entities;
using NSubstitute;
using Xunit;

namespace CoreFitnessClub.Tests.Unit.Application;

public class ManageTrainingClassServiceTests
{
    [Fact]
    public async Task CreateAsync_WithMissingName_ReturnsFailure()
    {
        var repository = Substitute.For<ITrainingClassRepository>();
        var service = new ManageTrainingClassService(repository);

        var request = new CreateTrainingClassRequest
        {
            Name = "",
            Category = "Yoga",
            InstructorName = "Anna Berg",
            Date = new DateOnly(2026, 5, 3),
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(11, 0)
        };

        var result = await service.CreateAsync(request);

        Assert.False(result.Succeeded);
        Assert.Equal("Class name is required.", result.ErrorMessage);

        await repository.DidNotReceive().AddAsync(
            Arg.Any<TrainingClass>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateAsync_WithMissingCategory_ReturnsFailure()
    {
        var repository = Substitute.For<ITrainingClassRepository>();
        var service = new ManageTrainingClassService(repository);

        var request = new CreateTrainingClassRequest
        {
            Name = "Morning Yoga",
            Category = "",
            InstructorName = "Anna Berg",
            Date = new DateOnly(2026, 5, 3),
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(11, 0)
        };

        var result = await service.CreateAsync(request);

        Assert.False(result.Succeeded);
        Assert.Equal("Category is required.", result.ErrorMessage);

        await repository.DidNotReceive().AddAsync(
            Arg.Any<TrainingClass>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateAsync_WithMissingInstructorName_ReturnsFailure()
    {
        var repository = Substitute.For<ITrainingClassRepository>();
        var service = new ManageTrainingClassService(repository);

        var request = new CreateTrainingClassRequest
        {
            Name = "Morning Yoga",
            Category = "Yoga",
            InstructorName = "",
            Date = new DateOnly(2026, 5, 3),
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(11, 0)
        };

        var result = await service.CreateAsync(request);

        Assert.False(result.Succeeded);
        Assert.Equal("Instructor name is required.", result.ErrorMessage);

        await repository.DidNotReceive().AddAsync(
            Arg.Any<TrainingClass>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateAsync_WithInvalidTime_ReturnsFailure()
    {
        var repository = Substitute.For<ITrainingClassRepository>();
        var service = new ManageTrainingClassService(repository);

        var request = new CreateTrainingClassRequest
        {
            Name = "Morning Yoga",
            Category = "Yoga",
            InstructorName = "Anna Berg",
            Date = new DateOnly(2026, 5, 3),
            StartTime = new TimeOnly(11, 0),
            EndTime = new TimeOnly(10, 0)
        };

        var result = await service.CreateAsync(request);

        Assert.False(result.Succeeded);
        Assert.Equal("End time must be later than start time.", result.ErrorMessage);

        await repository.DidNotReceive().AddAsync(
            Arg.Any<TrainingClass>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_ReturnsSuccess()
    {
        var repository = Substitute.For<ITrainingClassRepository>();
        var service = new ManageTrainingClassService(repository);

        var request = new CreateTrainingClassRequest
        {
            Name = " Morning Yoga ",
            Category = " Yoga ",
            InstructorName = " Anna Berg ",
            Date = new DateOnly(2026, 5, 3),
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(11, 0)
        };

        var result = await service.CreateAsync(request);

        Assert.True(result.Succeeded);
        Assert.Null(result.ErrorMessage);

        await repository.Received(1).AddAsync(
            Arg.Is<TrainingClass>(trainingClass =>
                trainingClass.Name == "Morning Yoga" &&
                trainingClass.Category == "Yoga" &&
                trainingClass.InstructorName == "Anna Berg" &&
                trainingClass.StartsAt == new DateTime(2026, 5, 3, 10, 0, 0) &&
                trainingClass.EndsAt == new DateTime(2026, 5, 3, 11, 0, 0)),
            Arg.Any<CancellationToken>());

        await repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteAsync_WhenClassDoesNotExist_ReturnsFailure()
    {
        var repository = Substitute.For<ITrainingClassRepository>();

        repository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns((TrainingClass?)null);

        var service = new ManageTrainingClassService(repository);

        var result = await service.DeleteAsync(1);

        Assert.False(result.Succeeded);
        Assert.Equal("Class not found.", result.ErrorMessage);

        repository.DidNotReceive().Remove(Arg.Any<TrainingClass>());
        await repository.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteAsync_WhenClassExists_ReturnsSuccess()
    {
        var repository = Substitute.For<ITrainingClassRepository>();

        var trainingClass = TrainingClass.Create(
            "Morning Yoga",
            "Yoga",
            "Anna Berg",
            new DateTime(2026, 5, 3, 10, 0, 0),
            new DateTime(2026, 5, 3, 11, 0, 0));

        repository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(trainingClass);

        var service = new ManageTrainingClassService(repository);

        var result = await service.DeleteAsync(1);

        Assert.True(result.Succeeded);
        Assert.Null(result.ErrorMessage);

        repository.Received(1).Remove(trainingClass);
        await repository.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}