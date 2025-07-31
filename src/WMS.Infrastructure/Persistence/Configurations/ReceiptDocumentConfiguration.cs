using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Domain.Common.ValueObjects;
using WMS.Domain.ReceiptDocumentAggregate;
using WMS.Domain.ReceiptDocumentAggregate.ValueObjects;
using WMS.Domain.ResourceAggregate.ValueObjects;
using WMS.Domain.UnitOfMeasurementAggregate.ValueObjects;

namespace WMS.Infrastructure.Persistence.Configurations;

public class ReceiptDocumentConfiguration : IEntityTypeConfiguration<ReceiptDocument>
{
    public void Configure(EntityTypeBuilder<ReceiptDocument> builder)
    {
        builder.ToTable("ReceiptDocuments");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => ReceiptDocumentId.Create(value)
            );

        builder.Property(r => r.Number)
            .HasConversion(
                num => num.Value,
                value => DocumentNumber.CreateNew(value)
            )
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(r => r.Date)
            .IsRequired();
        builder.OwnsMany(r => r.ReceiptResources, rr =>
        {
            rr.ToTable("ReceiptResources");

            rr.WithOwner().HasForeignKey("ReceiptDocumentId");

            rr.HasKey("Id");

            rr.Property(r => r.Id)
                .ValueGeneratedNever()
                .HasColumnName("Id")
                .HasConversion(
                    id => id.Value,
                    value => ReceiptResourceId.Create(value)
                );

            rr.Property(r => r.ResourceId)
                .HasConversion(
                    id => id.Value,
                    value => ResourceId.Create(value)
                )
                .IsRequired();

            rr.Property(r => r.UnitOfMeasurementId)
                .HasConversion(
                    id => id.Value,
                    value => UnitOfMeasurementId.Create(value)
                )
                .IsRequired();

            rr.OwnsOne(r => r.Quantity, q =>
            {
                q.Property(p => p.Value)
                    .HasColumnName("Quantity")
                    .IsRequired();
            });

            rr.HasIndex("ReceiptDocumentId");
        });
    }
}