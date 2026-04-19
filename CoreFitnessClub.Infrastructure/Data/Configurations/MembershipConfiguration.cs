using CoreFitnessClub.Domain.Entities;
using CoreFitnessClub.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreFitnessClub.Infrastructure.Data.Configurations;

public class MembershipConfiguration : IEntityTypeConfiguration<Membership>
{
    public void Configure(EntityTypeBuilder<Membership> builder)
    {
        builder.ToTable("Memberships");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(x => x.MembershipPlanId)
            .IsRequired();

        builder.Property(x => x.StartDate)
            .IsRequired();

        builder.HasIndex(x => x.UserId)
            .IsUnique();

        builder.HasOne<AppUser>()
            .WithOne()
            .HasForeignKey<Membership>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<MembershipPlan>()
            .WithMany()
            .HasForeignKey(x => x.MembershipPlanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired();

        builder.Property(x => x.UpdatedAtUtc)
            .IsRequired();

        builder.Property(x => x.RowVersion)
            .IsRowVersion();
    }
}