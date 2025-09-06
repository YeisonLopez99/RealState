using MediatR;
using FluentValidation;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceRegistration).Assembly));
        services.AddAutoMapper(typeof(ServiceRegistration).Assembly);
        services.AddValidatorsFromAssembly(typeof(ServiceRegistration).Assembly);
        return services;
    }
}