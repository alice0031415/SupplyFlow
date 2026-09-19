using SupplyFlow.Supplier.Infrastructure.Messaging;
using SupplyFlow.Supplier.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddMessaging(builder.Configuration);

var app = builder.Build();

app.MapControllers();

app.Run();