using Microsoft.EntityFrameworkCore;
using WMS.Domain.Common.Models;
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