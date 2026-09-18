using Microsoft.Extensions.DependencyInjection;
using SupplyFlow.Procurement.Application.Needs.Commands;

namespace SupplyFlow.Procurement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblyContaining<CreateNeedHandler>());

        return services;
    }
}