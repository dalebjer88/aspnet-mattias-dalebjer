using CoreFitnessClub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreFitnessClub.Infrastructure.Data.Configurations;

public class MembershipPlanFeatureConfiguration : IEntityTypeConfiguration<MembershipPlanFeature>
{
    public void Configure(EntityTypeBuilder<MembershipPlanFeature> builder)
    {
        builder.ToTable("MembershipPlanFeatures");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.MembershipPlanId)
            .IsRequired();

        builder.Property(x => x.Text)
            .IsRequired()
            .HasMaxLength(200);
    }
}