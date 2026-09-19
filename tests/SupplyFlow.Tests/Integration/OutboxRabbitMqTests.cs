using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SupplyFlow.Contracts.Events;
using SupplyFlow.Procurement.Infrastructure.Persistence;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace SupplyFlow.Tests.Integration;

public sealed class OutboxRabbitMqTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres =
        new PostgreSqlBuilder()
            .WithImage("postgres:17-alpine")
            .WithDatabase("supplyflow_test")
            .WithUsername("supplyflow")
            .WithPassword("supplyflow")
            .Build();

    private readonly RabbitMqContainer _rabbitMq =
        new RabbitMqBuilder()
            .WithImage("rabbitmq:4.3-management")
            .WithUsername("supplyflow")
            .WithPassword("supplyflow")
            .Build();

    private IHost? _host;

    public async ValueTask InitializeAsync()
    {
        await _postgres.StartAsync();
        await _rabbitMq.StartAsync();

        _host = Microsoft.Extensions.Hosting.Host
            .CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton<MessageProbe>();

                services.AddDbContext<SupplyFlowDbContext>(
                    options =>
                        options.UseNpgsql(
                            _postgres.GetConnectionString()));

                services.AddMassTransit(x =>
                {
                    x.AddConsumer<TestTenderConsumer>();

                    x.AddEntityFrameworkOutbox<
                        SupplyFlowDbContext>(o =>
                        {
                            o.UsePostgres();
                            o.UseBusOutbox();
                        });

                    x.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host(
                            _rabbitMq.Hostname,
                            _rabbitMq.GetMappedPublicPort(5672),
                            "/",
                            h =>
                            {
                                h.Username("supplyflow");
                                h.Password("supplyflow");
                            });

                        cfg.ReceiveEndpoint(
                            "supplyflow-outbox-test",
                            endpoint =>
                            {
                                endpoint.ConfigureConsumer<
                                    TestTenderConsumer>(context);
                            });
                    });
                });
            })
            .Build();

        await _host.StartAsync();

        await using var db =
            _host.Services
                .GetRequiredService<SupplyFlowDbContext>();

        await db.Database.EnsureCreatedAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (_host is not null)
        {
            await _host.StopAsync();
            _host.Dispose();
        }

        await _rabbitMq.DisposeAsync();
        await _postgres.DisposeAsync();
    }

    [Fact]
    public async Task Should_PublishThroughTransactionalOutbox()
    {
        var needId = Guid.NewGuid();

        await using var scope =
            _host!.Services.CreateAsyncScope();

        var publishEndpoint =
            scope.ServiceProvider
                .GetRequiredService<IPublishEndpoint>();

        await using var db =
            scope.ServiceProvider
                .GetRequiredService<SupplyFlowDbContext>();

        await publishEndpoint.Publish(
            new TenderPublishedIntegrationEvent
            {
                NeedId = needId,
                PublishedAtUtc = DateTime.UtcNow,
                CorrelationId = needId
            });

        await db.SaveChangesAsync();

        var probe =
            scope.ServiceProvider
                .GetRequiredService<MessageProbe>();

        var message = await probe.MessageReceived
            .Task
            .WaitAsync(TimeSpan.FromSeconds(10));

        Assert.Equal(needId, message.NeedId);
    }

    [Fact]
    public async Task Should_NotDeliverMessage_WhenTransactionRolledBack()
    {
        var needId = Guid.NewGuid();

        await using var scope =
            _host!.Services.CreateAsyncScope();

        var publishEndpoint =
            scope.ServiceProvider
                .GetRequiredService<IPublishEndpoint>();

        await using var db =
            scope.ServiceProvider
                .GetRequiredService<SupplyFlowDbContext>();

        await using var transaction =
            await db.Database.BeginTransactionAsync();

        await publishEndpoint.Publish(
            new TenderPublishedIntegrationEvent
            {
                NeedId = needId,
                PublishedAtUtc = DateTime.UtcNow,
                CorrelationId = needId
            });

        await db.SaveChangesAsync();

        await transaction.RollbackAsync();

        var probe =
            scope.ServiceProvider
                .GetRequiredService<MessageProbe>();

        await Assert.ThrowsAsync<TimeoutException>(
            async () =>
                await probe.MessageReceived.Task
                    .WaitAsync(TimeSpan.FromSeconds(2)));
    }

    private sealed class MessageProbe
    {
        public TaskCompletionSource<
            TenderPublishedIntegrationEvent> MessageReceived
        { get; } =
            new(TaskCreationOptions.RunContinuationsAsynchronously);
    }

    private sealed class TestTenderConsumer(
        MessageProbe probe)
        : IConsumer<TenderPublishedIntegrationEvent>
    {
        public Task Consume(
            ConsumeContext<TenderPublishedIntegrationEvent> context)
        {
            probe.MessageReceived.TrySetResult(
                context.Message);

            return Task.CompletedTask;
        }
    }
}