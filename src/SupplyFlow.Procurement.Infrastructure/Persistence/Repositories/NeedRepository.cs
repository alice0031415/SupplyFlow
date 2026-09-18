using Microsoft.EntityFrameworkCore;
using SupplyFlow.Procurement.Application.Needs;
using SupplyFlow.Procurement.Domain.Needs;

namespace SupplyFlow.Procurement.Infrastructure.Persistence.Repositories;

public sealed class NeedRepository(
    SupplyFlowDbContext dbContext) : INeedRepository
{
    public async Task AddAsync(
        Need need,
        CancellationToken cancellationToken)
    {
        await dbContext.Needs.AddAsync(need, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}