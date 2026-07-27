using Gymak.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gymak.Infrastructure.Persistence.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.FirstName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(m => m.LastName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(m => m.Email)
            .HasMaxLength(150)
            .IsRequired();

        builder.HasIndex(m => m.Email)
            .IsUnique();

        builder.OwnsOne(m => m.Address, a =>
        {
            a.Property(x => x.Street).HasMaxLength(100);
            a.Property(x => x.City).HasMaxLength(50);
            a.Property(x => x.State).HasMaxLength(50);
            a.Property(x => x.ZipCode).HasMaxLength(20);
        });
    }
}
