using CoreFitnessClub.Application.Features.Classes;
using CoreFitnessClub.Domain.Entities;
using CoreFitnessClub.Infrastructure.Data;
using CoreFitnessClub.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CoreFitnessClub.Tests.Integration;

public class TrainingClassIntegrationTests
{
    [Fact]
    public async Task GetAvailableAsync_ReturnsOnlyFutureClasses()
    {
        await using var dbContext = CreateDbContext();

        var pastClass = TrainingClass.Create(
            "Past Yoga",
            "Yoga",
            "Anna Berg",
            DateTime.Now.AddDays(-1),
            DateTime.Now.AddDays(-1).AddHours(1));

        var futureClass = TrainingClass.Create(
            "Future Yoga",
            "Yoga",
            "Anna Berg",
            DateTime.Now.AddDays(1),
            DateTime.Now.AddDays(1).AddHours(1));

        await dbContext.TrainingClasses.AddRangeAsync(pastClass, futureClass);
        await dbContext.SaveChangesAsync();

        var repository = new TrainingClassRepository(dbContext);
        var service = new ReadTrainingClassService(repository);

        var result = await service.GetAvailableAsync();

        Assert.Single(result);
        Assert.Equal("Future Yoga", result[0].Name);
    }

    [Fact]
    public async Task GetAvailableAsync_OrdersClassesByStartTime()
    {
        await using var dbContext = CreateDbContext();

        var laterClass = TrainingClass.Create(
            "Later Class",
            "Strength",
            "Erik Nilsson",
            DateTime.Now.AddDays(3),
            DateTime.Now.AddDays(3).AddHours(1));

        var earlierClass = TrainingClass.Create(
            "Earlier Class",
            "Yoga",
            "Anna Berg",
            DateTime.Now.AddDays(1),
            DateTime.Now.AddDays(1).AddHours(1));

        await dbContext.TrainingClasses.AddRangeAsync(laterClass, earlierClass);
        await dbContext.SaveChangesAsync();

        var repository = new TrainingClassRepository(dbContext);
        var service = new ReadTrainingClassService(repository);

        var result = await service.GetAvailableAsync();

        Assert.Equal(2, result.Count);
        Assert.Equal("Earlier Class", result[0].Name);
        Assert.Equal("Later Class", result[1].Name);
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_SavesTrainingClass()
    {
        await using var dbContext = CreateDbContext();

        var repository = new TrainingClassRepository(dbContext);
        var service = new ManageTrainingClassService(repository);

        var request = new CreateTrainingClassRequest
        {
            Name = " Morning Yoga ",
            Category = " Yoga ",
            InstructorName = " Anna Berg ",
            Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            StartTime = new TimeOnly(10, 0),
            EndTime = new TimeOnly(11, 0)
        };

        var result = await service.CreateAsync(request);

        var savedClass = await dbContext.TrainingClasses.SingleAsync();

        Assert.True(result.Succeeded);
        Assert.Null(result.ErrorMessage);
        Assert.Equal("Morning Yoga", savedClass.Name);
        Assert.Equal("Yoga", savedClass.Category);
        Assert.Equal("Anna Berg", savedClass.InstructorName);
        Assert.Equal(request.Date.ToDateTime(request.StartTime), savedClass.StartsAt);
        Assert.Equal(request.Date.ToDateTime(request.EndTime), savedClass.EndsAt);
    }

    [Fact]
    public async Task DeleteAsync_WhenClassExists_RemovesTrainingClass()
    {
        await using var dbContext = CreateDbContext();

        var trainingClass = TrainingClass.Create(
            "Morning Yoga",
            "Yoga",
            "Anna Berg",
            DateTime.Now.AddDays(1),
            DateTime.Now.AddDays(1).AddHours(1));

        await dbContext.TrainingClasses.AddAsync(trainingClass);
        await dbContext.SaveChangesAsync();

        var repository = new TrainingClassRepository(dbContext);
        var service = new ManageTrainingClassService(repository);

        var result = await service.DeleteAsync(trainingClass.Id);

        var classExists = await dbContext.TrainingClasses.AnyAsync();

        Assert.True(result.Succeeded);
        Assert.Null(result.ErrorMessage);
        Assert.False(classExists);
    }

    private static CoreFitnessClubDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<CoreFitnessClubDbContext>()
            .UseInMemoryDatabase($"CoreFitnessClubTests-{Guid.NewGuid()}")
            .Options;

        return new CoreFitnessClubDbContext(options);
    }
}