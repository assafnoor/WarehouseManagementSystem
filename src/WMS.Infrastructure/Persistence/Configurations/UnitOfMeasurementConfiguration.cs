using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Domain.UnitOfMeasurementAggregate;
using WMS.Domain.UnitOfMeasurementAggregate.ValueObjects;

namespace WMS.Infrastructure.Persistence.Configurations;

public class UnitOfMeasurementConfiguration : IEntityTypeConfiguration<UnitOfMeasurement>
{
    public void Configure(EntityTypeBuilder<UnitOfMeasurement> builder)
    {
        builder.ToTable("UnitsOfMeasurement");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => UnitOfMeasurementId.Create(value)
            );

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.IsActive)
            .IsRequired();

        // Optional index on Name if you want to search or prevent duplicates
        builder.HasIndex(u => u.Name).IsUnique();
    }
}