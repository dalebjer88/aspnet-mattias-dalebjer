using CoreFitnessClub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreFitnessClub.Infrastructure.Data.Configurations;

public class MemberProfileConfiguration : IEntityTypeConfiguration<MemberProfile>
{
    public void Configure(EntityTypeBuilder<MemberProfile> builder)
    {
        builder.ToTable("MemberProfiles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(30);

        builder.Property(x => x.ProfileImagePath)
            .HasMaxLength(500);

        builder.HasIndex(x => x.UserId)
            .IsUnique();
    }
}