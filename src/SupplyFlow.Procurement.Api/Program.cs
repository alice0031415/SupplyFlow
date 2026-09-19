using Serilog;
using SupplyFlow.Procurement.Api.Infrastructure;
using SupplyFlow.Procurement.Api.Observability;
using SupplyFlow.Procurement.Application;
using SupplyFlow.Procurement.Infrastructure.Grpc;
using SupplyFlow.Procurement.Infrastructure.Logistics;
using SupplyFlow.Procurement.Infrastructure.Messaging;
using SupplyFlow.Procurement.Infrastructure.Persistence;
using SupplyFlow.Procurement.Infrastructure.Redis;
using SupplyFlow.Procurement.Api.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(
    (context, configuration) =>
        configuration.ReadFrom.Configuration(
            context.Configuration));

builder.Services.AddControllers();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddMessaging(builder.Configuration);
builder.Services.AddGrpcClients();
builder.Services.AddLogisticsClient(builder.Configuration);
builder.Services.AddRedis(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services
    .AddHealthChecks()
    .AddDbContextCheck<SupplyFlowDbContext>();

builder.Services.AddObservability(
    builder.Configuration,
    builder.Environment);

builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");
app.MapPrometheusScrapingEndpoint("/metrics");

app.Run();