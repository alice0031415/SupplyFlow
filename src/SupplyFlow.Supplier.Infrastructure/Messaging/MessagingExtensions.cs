using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
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

        services.AddQuartz();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<TenderPublishedConsumer>();

            x.AddEntityFrameworkOutbox<SupplyFlowSupplierDbContext>(o =>
            {
                o.UsePostgres();
            });

            x.AddPublishMessageScheduler();
            x.AddQuartzConsumers();

            x.AddConfigureEndpointsCallback((context, _, endpoint) =>
            {
                endpoint.UseScheduledRedelivery(retry =>
                {
                    retry.Intervals(
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(15));
                });

                endpoint.UseMessageRetry(retry =>
                {
                    retry.Immediate(2);
                    retry.Handle<InvalidOperationException>();
                });

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

                cfg.UsePublishMessageScheduler();

                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        return services;
    }
}