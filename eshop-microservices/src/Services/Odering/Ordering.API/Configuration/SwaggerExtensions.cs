using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Ordering.API.Configuration;

internal static class SwaggerExtensions
{
    internal static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Ordering API",
                Version = "v1",
                Description = "Ordering API for managing customer orders"
            });
           
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(baseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });
        return services;
    }
    internal static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
    {
        app.UseSwagger();

        return app;
    }
}
