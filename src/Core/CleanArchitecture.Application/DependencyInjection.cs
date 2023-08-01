using System.Reflection;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using MediatR.Extensions.FluentValidation.AspNetCore;

namespace CleanArchitecture.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.AddFluentValidation(new[] { typeof(DependencyInjection).Assembly });
        
        services.AddMapster();
        
        return services;
    }
}