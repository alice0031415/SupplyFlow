using SupplyFlow.Supplier.Infrastructure.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddMessaging(builder.Configuration);

var app = builder.Build();

app.MapControllers();

app.Run();