using CoreFitnessClub.Domain.Entities;
using CoreFitnessClub.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreFitnessClub.Infrastructure.Data.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable("UserProfiles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(x => x.FirstName)
            .HasMaxLength(100);

        builder.Property(x => x.LastName)
            .HasMaxLength(100);

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(30);

        builder.Property(x => x.ProfileImagePath)
            .HasMaxLength(500);

        builder.HasIndex(x => x.UserId)
            .IsUnique();

        builder.HasOne<AppUser>()
            .WithOne()
            .HasForeignKey<UserProfile>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired();

        builder.Property(x => x.UpdatedAtUtc)
            .IsRequired();

        builder.Property(x => x.RowVersion)
            .IsRowVersion();
    }
}