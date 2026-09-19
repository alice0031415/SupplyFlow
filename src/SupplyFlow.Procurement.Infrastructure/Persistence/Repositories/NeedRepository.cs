using Microsoft.EntityFrameworkCore;
using SupplyFlow.Procurement.Application.DomainEvents;
using SupplyFlow.Procurement.Application.Needs;
using SupplyFlow.Procurement.Domain.Needs;

namespace SupplyFlow.Procurement.Infrastructure.Persistence.Repositories;

public sealed class NeedRepository(
    SupplyFlowDbContext dbContext,
    IDomainEventDispatcher eventDispatcher) : INeedRepository
{
    public async Task AddAsync(
        Need need,
        CancellationToken cancellationToken)
    {
        await dbContext.Needs.AddAsync(
            need,
            cancellationToken);

        await SaveChangesAsync(cancellationToken);
    }

    public Task<Need?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return dbContext.Needs
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<IReadOnlyList<NeedDto>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        return await dbContext.Needs
            .AsNoTracking()
            .OrderByDescending(x => x.RequiredBy)
            .Select(x => new NeedDto(
                x.Id,
                x.Description,
                x.Quantity,
                x.RequiredBy,
                x.Status))
            .ToListAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken)
    {
        var events = dbContext.ChangeTracker
            .Entries<Need>()
            .SelectMany(x => x.Entity.DomainEvents)
            .ToArray();

        if (events.Length > 0)
        {
            await eventDispatcher.DispatchAsync(
                events,
                cancellationToken);
        }

        await dbContext.SaveChangesAsync(
            cancellationToken);

        foreach (var entry in dbContext.ChangeTracker.Entries<Need>())
        {
            entry.Entity.ClearDomainEvents();
        }
    }
}