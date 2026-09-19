using Microsoft.EntityFrameworkCore;
using SupplyFlow.Procurement.Domain.Needs;
using SupplyFlow.Procurement.Infrastructure.Persistence;
using Testcontainers.PostgreSql;

namespace SupplyFlow.Tests.Integration;

public sealed class ProcurementDatabaseTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres =
        new PostgreSqlBuilder()
            .WithImage("postgres:17-alpine")
            .WithDatabase("supplyflow_test")
            .WithUsername("supplyflow")
            .WithPassword("supplyflow")
            .Build();

    public async ValueTask InitializeAsync()
    {
        await _postgres.StartAsync();

        await using var db = CreateDbContext();

        await db.Database.MigrateAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _postgres.DisposeAsync();
    }

    [Fact]
    public async Task Should_SaveAndReadNeed()
    {
        // Arrange
        var need = new Need(
            "Мониторы",
            5,
            new DateOnly(2026, 10, 20));

        // Act
        await using (var db = CreateDbContext())
        {
            db.Needs.Add(need);

            await db.SaveChangesAsync();
        }

        // Assert
        await using (var db = CreateDbContext())
        {
            var saved = await db.Needs
                .AsNoTracking()
                .SingleAsync(x => x.Id == need.Id);

            Assert.Equal(need.Id, saved.Id);
            Assert.Equal("Мониторы", saved.Description);
            Assert.Equal(5, saved.Quantity);
            Assert.Equal(NeedStatus.Draft, saved.Status);
        }
    }

    private SupplyFlowDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<SupplyFlowDbContext>()
            .UseNpgsql(_postgres.GetConnectionString())
            .Options;

        return new SupplyFlowDbContext(options);
    }
}