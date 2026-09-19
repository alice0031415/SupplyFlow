using Microsoft.AspNetCore.Server.Kestrel.Core;
using SupplyFlow.Supplier.Api.Grpc;
using SupplyFlow.Supplier.Infrastructure.Messaging;
using SupplyFlow.Supplier.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(
        5002,
        listenOptions =>
        {
            listenOptions.Protocols = HttpProtocols.Http2;
        });
});

builder.Services.AddControllers();
builder.Services.AddGrpc();

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddMessaging(builder.Configuration);

var app = builder.Build();

app.MapControllers();
app.MapGrpcService<SupplierDirectoryService>();

app.Run();