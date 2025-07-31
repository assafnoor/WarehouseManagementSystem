using Microsoft.AspNetCore.Mvc.Infrastructure;
using WMS.Api.Common.Errors;

namespace WMS.Api;

public static class DependencyInjectionRegister
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddOpenApi();
        services.AddSingleton<ProblemDetailsFactory, MyAppProblemDetailsFactory>();

        services.AddEndpointsApiExplorer();
        //services.AddSwaggerGen(d =>
        //{
        //    d.SwaggerDoc("Error", new OpenApiInfo
        //    {
        //        Title = "Error",
        //        Version = "v1",
        //        Description = "The Error Controller",
        //    });

        //    d.SwaggerDoc("Auth", new OpenApiInfo
        //    {
        //        Title = "Authentication",
        //        Version = "v1",
        //        Description = "Authentication Controller",
        //    });
        //});
        return services;
    }
}