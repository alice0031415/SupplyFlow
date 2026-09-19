using MassTransit;
using Microsoft.EntityFrameworkCore;
using SupplyFlow.Supplier.Domain.Tenders;

namespace SupplyFlow.Supplier.Infrastructure.Persistence;

public sealed class SupplyFlowSupplierDbContext(
    DbContextOptions<SupplyFlowSupplierDbContext> options)
    : DbContext(options)
{
    public DbSet<ReceivedTender> ReceivedTenders =>
        Set<ReceivedTender>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReceivedTender>(builder =>
        {
            builder.ToTable("received_tenders");

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.NeedId)
                .IsUnique();
        });

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}