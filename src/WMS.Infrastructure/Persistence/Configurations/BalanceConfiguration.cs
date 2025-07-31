using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Domain.BalanceAggregate;
using WMS.Domain.BalanceAggregate.ValueObjects;
using WMS.Domain.ResourceAggregate.ValueObjects;
using WMS.Domain.UnitOfMeasurementAggregate.ValueObjects;

namespace WMS.Infrastructure.Persistence.Configurations;

public class BalanceConfiguration : IEntityTypeConfiguration<Balance>
{
    public void Configure(EntityTypeBuilder<Balance> builder)
    {
        builder.ToTable("Balances");

        // Primary Key
        builder
    .HasKey(b => b.Id);

        builder
            .Property(b => b.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => BalanceId.Create(value));

        // Foreign key: ResourceId
        builder.Property(b => b.ResourceId)
            .HasConversion(
                id => id.Value,
                value => ResourceId.Create(value)
            );

        // Foreign key: UnitOfMeasurementId
        builder.Property(b => b.UnitOfMeasurementId)
            .HasConversion(
                id => id.Value,
                value => UnitOfMeasurementId.Create(value)
            );

        // Quantity ValueObject mapping (decimal)
        builder.OwnsOne(b => b.Quantity, q =>
        {
            q.Property(q => q.Value)
                .HasColumnName("Quantity")
                .IsRequired();
        });

        // Index for performance (optional)
        builder.HasIndex(b => new { b.ResourceId, b.UnitOfMeasurementId })
            .IsUnique(); // Assuming each resource + unit combo has one balance
    }
}