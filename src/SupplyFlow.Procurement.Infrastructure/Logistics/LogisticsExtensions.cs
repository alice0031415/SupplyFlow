using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SupplyFlow.Procurement.Application.Logistics;
using Refit;

namespace SupplyFlow.Procurement.Infrastructure.Logistics;

public static class LogisticsExtensions
{
    public static IServiceCollection AddLogisticsClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var baseAddress =
            configuration["Logistics:BaseAddress"]
            ?? "http://localhost:5003";

        services
            .AddRefitClient<ILogisticsApi>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri(baseAddress);
            })
            .AddStandardResilienceHandler(options =>
            {
                options.TotalRequestTimeout.Timeout =
                    TimeSpan.FromSeconds(5);

                options.AttemptTimeout.Timeout =
                    TimeSpan.FromSeconds(1);

                options.Retry.MaxRetryAttempts = 2;

                options.Retry.Delay =
                    TimeSpan.FromMilliseconds(200);

                options.CircuitBreaker.FailureRatio = 0.5;

                options.CircuitBreaker.MinimumThroughput = 2;

                options.CircuitBreaker.SamplingDuration =
                    TimeSpan.FromSeconds(10);

                options.CircuitBreaker.BreakDuration =
                    TimeSpan.FromSeconds(5);
            });

        services.AddScoped<
            ILogisticsGateway,
            LogisticsGateway>();

        return services;
    }
}