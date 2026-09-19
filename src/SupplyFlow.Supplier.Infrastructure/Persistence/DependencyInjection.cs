using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SupplyFlow.Supplier.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("SupplyFlowSupplier");

        services.AddDbContext<SupplyFlowSupplierDbContext>(
            options => options.UseNpgsql(connectionString));

        return services;
    }
}