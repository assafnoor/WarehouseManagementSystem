using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS.Domain.ClientAggregate;
using WMS.Domain.ClientAggregate.ValueObjects;

namespace WMS.Infrastructure.Persistence.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clients");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => ClientId.Create(value)
            );

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.IsActive)
            .IsRequired();

        // Address value object mapping
        builder.OwnsOne(c => c.Address, addr =>
        {
            addr.Property(a => a.Street)
                .HasColumnName("Street")
                .HasMaxLength(200);

            addr.Property(a => a.City)
                .HasColumnName("City")
                .HasMaxLength(100);

            addr.Property(a => a.PostalCode)
                .HasColumnName("PostalCode")
                .HasMaxLength(20);
        });
    }
}