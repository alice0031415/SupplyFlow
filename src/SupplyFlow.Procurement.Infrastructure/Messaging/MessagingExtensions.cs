using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SupplyFlow.Procurement.Application.Needs.Events;
using SupplyFlow.Procurement.Infrastructure.Persistence;

namespace SupplyFlow.Procurement.Infrastructure.Messaging;

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
            x.AddEntityFrameworkOutbox<SupplyFlowDbContext>(o =>
            {
                o.UsePostgres();
                o.UseBusOutbox();
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

        services.AddScoped<
            ITenderPublishedPublisher,
            TenderPublishedPublisher>();

        return services;
    }
}