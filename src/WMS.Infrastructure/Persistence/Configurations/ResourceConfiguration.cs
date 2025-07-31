using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Domain.ResourceAggregate;
using WMS.Domain.ResourceAggregate.ValueObjects;

namespace WMS.Infrastructure.Persistence.Configurations;

public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
{
    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        builder.ToTable("Resources");

        // Primary Key
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => ResourceId.Create(value)
            );

        builder.Property(r => r.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(r => r.IsActive)
            .IsRequired();

        // Ignore domain events
        builder.Ignore(r => r.DomainEvents);
    }
}