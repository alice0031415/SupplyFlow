using Microsoft.Extensions.DependencyInjection;
using SupplyFlow.Contracts.Grpc;
using SupplyFlow.Procurement.Application.Suppliers;

namespace SupplyFlow.Procurement.Infrastructure.Grpc;

public static class GrpcExtensions
{
    public static IServiceCollection AddGrpcClients(
        this IServiceCollection services)
    {
        services.AddGrpcClient<SupplierDirectory.SupplierDirectoryClient>(
            options =>
            {
                options.Address = new Uri("http://localhost:5002");
            });

        services.AddScoped<ISupplierGateway, SupplierGateway>();

        return services;
    }
}