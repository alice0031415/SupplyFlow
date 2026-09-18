using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SupplyFlow.Procurement.Application.Needs;
using SupplyFlow.Procurement.Infrastructure.Persistence.Repositories;

namespace SupplyFlow.Procurement.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("SupplyFlow");

        services.AddDbContext<SupplyFlowDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<INeedRepository, NeedRepository>();

        return services;
    }
}