using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DMS.DAL.Persistence.Configurations;

public class DeviceAssignmentConfiguration : IEntityTypeConfiguration<DeviceAssignment>
{
    public void Configure(EntityTypeBuilder<DeviceAssignment> builder)
    {
        builder.ToTable("DeviceAssignments");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.AssignedAtUtc)
            .IsRequired();

        builder.Property(a => a.ReturnedAt);

        builder.Property(a => a.Notes)
            .HasMaxLength(1000);

        // IsActive is computed — not mapped to a column
        builder.Ignore(a => a.IsActive);

        builder.HasOne(a => a.Device)
            .WithMany(d => d.Assignments)
            .HasForeignKey(a => a.DeviceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.User)
            .WithMany(u => u.Assignments)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}