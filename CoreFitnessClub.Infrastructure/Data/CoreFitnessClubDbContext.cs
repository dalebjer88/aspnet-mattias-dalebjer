using CoreFitnessClub.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoreFitnessClub.Infrastructure.Data;

public class CoreFitnessClubDbContext : IdentityDbContext<AppUser>
{
    public CoreFitnessClubDbContext(DbContextOptions<CoreFitnessClubDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoreFitnessClubDbContext).Assembly);
    }
}