using CoreFitnessClub.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CoreFitnessClub.Infrastructure.Data;

public class CoreFitnessClubDbContextFactory : IDesignTimeDbContextFactory<CoreFitnessClubDbContext>
{
    public CoreFitnessClubDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CoreFitnessClubDbContext>();

        var connectionString =
            "Server=(localdb)\\MSSQLLocalDB;Database=CoreFitnessClubDb;Trusted_Connection=True;TrustServerCertificate=True;";

        optionsBuilder.UseSqlServer(connectionString);

        return new CoreFitnessClubDbContext(optionsBuilder.Options);
    }
}