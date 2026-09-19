using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SupplyFlow.Supplier.Infrastructure.Messaging.Consumers;
using SupplyFlow.Supplier.Infrastructure.Persistence;

namespace SupplyFlow.Supplier.Infrastructure.Messaging;

public static class MessagingExtensions
{
    public static IServiceCollection AddMessaging(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var rabbitMq = configuration.GetSection("RabbitMQ");

        var host = rabbitMq["Host"] ?? "localhost";
        var username = rabbitMq["Username"] ?? "supplyflow";
        var password = rabbitMq["Password"] ?? "supplyflow";

        services.AddMassTransit(x =>
        {
            x.AddConsumer<TenderPublishedConsumer>();

            x.AddEntityFrameworkOutbox<SupplyFlowSupplierDbContext>(o =>
            {
                o.UsePostgres();
            });

            x.AddConfigureEndpointsCallback((context, _, endpoint) =>
            {
                endpoint.UseEntityFrameworkOutbox<
                    SupplyFlowSupplierDbContext>(context);
            });

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(host, "/", h =>
                {
                    h.Username(username);
                    h.Password(password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}