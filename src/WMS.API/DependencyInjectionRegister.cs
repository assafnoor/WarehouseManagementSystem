using Microsoft.AspNetCore.Mvc.Infrastructure;
using MyApp.Api.Common.Mapping;
using WMS.Api.Common.Errors;

namespace WMS.Api;

public static class DependencyInjectionRegister
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddSingleton<ProblemDetailsFactory, MyAppProblemDetailsFactory>();
        services.AddMappings();
        services.AddHttpContextAccessor();
        services.AddOpenApi();

        return services;
    }
}