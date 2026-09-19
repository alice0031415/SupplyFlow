using SupplyFlow.Procurement.Api.Infrastructure;
using SupplyFlow.Procurement.Application;
using SupplyFlow.Procurement.Infrastructure.Messaging;
using SupplyFlow.Procurement.Infrastructure.Persistence;
using SupplyFlow.Procurement.Infrastructure.Grpc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddMessaging(builder.Configuration);
builder.Services.AddGrpcClients();

builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

app.MapControllers();

app.Run();