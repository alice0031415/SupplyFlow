using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SupplyFlow.Procurement.Application.Behaviors;
using SupplyFlow.Procurement.Application.Needs.Commands;

namespace SupplyFlow.Procurement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<CreateNeedHandler>();

            cfg.AddOpenBehavior(
                typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssemblyContaining<CreateNeedValidator>();

        return services;
    }
}