using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Domain.ClientAggregate.ValueObjects;
using WMS.Domain.Common.ValueObjects;
using WMS.Domain.ResourceAggregate.ValueObjects;
using WMS.Domain.ShipmentDocumentAggregate;
using WMS.Domain.ShipmentDocumentAggregate.ValueObjects;
using WMS.Domain.UnitOfMeasurementAggregate.ValueObjects;

namespace WMS.Infrastructure.Persistence.Configurations;

public class ShipmentDocumentConfiguration : IEntityTypeConfiguration<ShipmentDocument>
{
    public void Configure(EntityTypeBuilder<ShipmentDocument> builder)
    {
        builder.ToTable("ShipmentDocuments");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => ShipmentDocumentId.Create(value)
            );

        builder.Property(s => s.Number)
            .HasConversion(
                num => num.Value,
                value => DocumentNumber.CreateNew(value)
            )
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(s => s.ClientId)
            .HasConversion(
                id => id.Value,
                value => ClientId.Create(value)
            )
            .IsRequired();

        builder.Property(s => s.Date)
            .IsRequired();

        builder.Property(s => s.Status)
            .HasConversion<int>() // Enum to int
            .IsRequired();

        // Owned collection
        builder.OwnsMany(s => s.ShipmentResources, sr =>
        {
            sr.ToTable("ShipmentResources");

            sr.WithOwner().HasForeignKey("ShipmentDocumentId");

            sr.HasKey("Id");

            sr.Property(s => s.Id)
                .ValueGeneratedNever()
                .HasColumnName("Id")
                .HasConversion(
                    id => id.Value,
                    value => ShipmentResourceId.Create(value)
                );

            sr.Property(s => s.ResourceId)
                .HasConversion(
                    id => id.Value,
                    value => ResourceId.Create(value)
                )
                .IsRequired();

            sr.Property(s => s.UnitOfMeasurementId)
                .HasConversion(
                    id => id.Value,
                    value => UnitOfMeasurementId.Create(value)
                )
                .IsRequired();

            sr.OwnsOne(r => r.Quantity, q =>
            {
                q.Property(p => p.Value)
                    .HasColumnName("Quantity")
                    .IsRequired();
            });

            sr.Property<Guid>("ShipmentDocumentId");
            sr.HasIndex("ShipmentDocumentId");
        });
    }
}