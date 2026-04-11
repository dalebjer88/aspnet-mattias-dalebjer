using CoreFitnessClub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreFitnessClub.Infrastructure.Data.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.TrainingClassId)
            .IsRequired();

        builder.Property(x => x.BookedAt)
            .IsRequired();

        builder.HasIndex(x => new { x.UserId, x.TrainingClassId })
            .IsUnique();

        builder.HasOne<TrainingClass>()
            .WithMany()
            .HasForeignKey(x => x.TrainingClassId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}