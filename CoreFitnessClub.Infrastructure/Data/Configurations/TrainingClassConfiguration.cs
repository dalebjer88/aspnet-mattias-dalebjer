using CoreFitnessClub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreFitnessClub.Infrastructure.Data.Configurations;

public class TrainingClassConfiguration : IEntityTypeConfiguration<TrainingClass>
{
    public void Configure(EntityTypeBuilder<TrainingClass> builder)
    {
        builder.ToTable("TrainingClasses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Category)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.InstructorName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.StartsAt)
            .IsRequired();

        builder.Property(x => x.EndsAt)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired();

        builder.Property(x => x.UpdatedAtUtc)
            .IsRequired();

        builder.Property(x => x.RowVersion)
            .IsRowVersion();
    }
}