using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DMS.DAL.Persistence.Configurations;

public class InviteConfiguration : IEntityTypeConfiguration<Invite>
{
    public void Configure(EntityTypeBuilder<Invite> builder)
    {
        builder.ToTable("Invites");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(i => i.Token)
            .IsRequired()
            .HasMaxLength(512);

        builder.HasIndex(i => i.Token).IsUnique();

        builder.Property(i => i.Role)
            .IsRequired();

        builder.Property(i => i.IsUsed)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(i => i.ExpiresAtUtc)
            .IsRequired();

        builder.Property(i => i.CreatedAtUtc)
            .IsRequired();

        builder.HasOne(i => i.CreatedByUser)
            .WithMany()
            .HasForeignKey(i => i.CreatedByUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}