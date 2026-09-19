using MassTransit;
using Microsoft.EntityFrameworkCore;
using SupplyFlow.Procurement.Domain.Needs;

namespace SupplyFlow.Procurement.Infrastructure.Persistence;

public class SupplyFlowDbContext(
    DbContextOptions<SupplyFlowDbContext> options) : DbContext(options)
{
    public DbSet<Need> Needs => Set<Need>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(SupplyFlowDbContext).Assembly);

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}