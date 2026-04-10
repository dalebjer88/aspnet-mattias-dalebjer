using CoreFitnessClub.Domain.Entities;
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

    public DbSet<UserProfile> UserProfiles { get; set; } = null!;
    public DbSet<MembershipPlan> MembershipPlans { get; set; } = null!;
    public DbSet<MembershipPlanFeature> MembershipPlanFeatures { get; set; } = null!;
    public DbSet<Membership> Memberships { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoreFitnessClubDbContext).Assembly);
    }
}