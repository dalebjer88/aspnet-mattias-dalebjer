using CoreFitnessClub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreFitnessClub.Infrastructure.Data.Configurations;

public class MembershipPlanConfiguration : IEntityTypeConfiguration<MembershipPlan>
{
    public void Configure(EntityTypeBuilder<MembershipPlan> builder)
    {
        builder.ToTable("MembershipPlans");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.PricePerMonth)
            .HasPrecision(18, 2);

        builder.Property(x => x.ClassesPerMonth)
            .IsRequired();

        builder.Property(x => x.TrialWeeks)
            .IsRequired();

        builder.HasMany<MembershipPlanFeature>()
            .WithOne()
            .HasForeignKey(x => x.MembershipPlanId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}