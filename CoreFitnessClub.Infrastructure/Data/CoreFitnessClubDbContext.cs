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
    public DbSet<TrainingClass> TrainingClasses { get; set; } = null!;
    public DbSet<Booking> Bookings { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoreFitnessClubDbContext).Assembly);
    }

    private void UpdateAuditFields()
    {
        var utcNow = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(nameof(BaseEntity.CreatedAtUtc)).CurrentValue = utcNow;
                entry.Property(nameof(BaseEntity.UpdatedAtUtc)).CurrentValue = utcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Property(nameof(BaseEntity.CreatedAtUtc)).IsModified = false;
                entry.Property(nameof(BaseEntity.UpdatedAtUtc)).CurrentValue = utcNow;
            }
        }
    }

    public override int SaveChanges()
    {
        UpdateAuditFields();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        UpdateAuditFields();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}