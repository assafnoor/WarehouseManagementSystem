using Microsoft.EntityFrameworkCore;
using WMS.Domain.BalanceAggregate;
using WMS.Domain.ClientAggregate;
using WMS.Domain.Common.Models;
using WMS.Domain.ReceiptDocumentAggregate;
using WMS.Domain.ResourceAggregate;
using WMS.Domain.ShipmentDocumentAggregate;
using WMS.Domain.UnitOfMeasurementAggregate;
using WMS.Infrastructure.Persistence.Interceptors;

namespace WMS.Infrastructure.Persistence;

public sealed class MyAppDbContext : DbContext
{
    private readonly PublishDomainEventsInterceptor _publishDomainEventsInterceptor;

    public MyAppDbContext(
        DbContextOptions<MyAppDbContext> options,
        PublishDomainEventsInterceptor publishDomainEventsInterceptor
    ) : base(options)
    {
        _publishDomainEventsInterceptor = publishDomainEventsInterceptor;
    }

    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<Resource> Resources { get; set; } = null!;
    public DbSet<ReceiptDocument> ReceiptDocuments { get; set; } = null!;

    public DbSet<ShipmentDocument> ShipmentDocuments { get; set; } = null!;
    public DbSet<Balance> Balances { get; set; } = null!;

    public DbSet<UnitOfMeasurement> UnitOfMeasurements { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Ignore<List<IDomainEvent>>()
            .ApplyConfigurationsFromAssembly(typeof(MyAppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .AddInterceptors(_publishDomainEventsInterceptor);

        base.OnConfiguring(optionsBuilder);
    }
}