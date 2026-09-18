using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        return services;
    }
}