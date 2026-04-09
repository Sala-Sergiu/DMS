using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DMS.DAL.Persistence.Configurations;

public class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.ToTable("Devices");
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name).IsRequired().HasMaxLength(200);
        builder.Property(d => d.SerialNumber).IsRequired().HasMaxLength(100);
        builder.HasIndex(d => d.SerialNumber).IsUnique();
        builder.Property(d => d.AssetTag).IsRequired().HasMaxLength(100);
        builder.HasIndex(d => d.AssetTag).IsUnique();
        builder.Property(d => d.Brand).IsRequired().HasMaxLength(100);
        builder.Property(d => d.Model).IsRequired().HasMaxLength(100);
        builder.Property(d => d.Type).IsRequired().HasConversion<int>();
        builder.Property(d => d.Status).IsRequired().HasConversion<int>();
        builder.Property(d => d.OperatingSystem).HasMaxLength(100);
        builder.Property(d => d.OsVersion).HasMaxLength(50);
        builder.Property(d => d.Processor).HasMaxLength(200);
        builder.Property(d => d.RamGb);
        builder.Property(d => d.Description).HasMaxLength(1000);
        builder.Property(d => d.PurchasedAtUtc);
        builder.Property(d => d.CreatedAtUtc).IsRequired();
        builder.Property(d => d.UpdatedAtUtc).IsRequired();

        builder.Navigation(d => d.Assignments)
            .HasField("_assignments")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(d => d.ActiveAssignment);
        builder.Ignore(d => d.IsAssigned);
    }
}