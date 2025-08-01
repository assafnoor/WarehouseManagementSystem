using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WMS.Application.Common.Interface.Persistence;
using WMS.Application.Services;
using WMS.Infrastructure.Persistence;
using WMS.Infrastructure.Persistence.Interceptors;
using WMS.Infrastructure.Persistence.Repositories;
using WMS.Infrastructure.Services;

namespace WMS.Infrastructure;

public static class DependencyInjectionRegister
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services
           .AddPersistance(configuration);

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }

    public static IServiceCollection AddPersistance(
     this IServiceCollection services,
      IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<MyAppDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<PublishDomainEventsInterceptor>();
        services.AddScoped<IResourceRepository, ResourceRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IUnitOfMeasurementRepository, UnitOfMeasurementRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}