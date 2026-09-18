using SupplyFlow.Procurement.Domain.Needs;

namespace SupplyFlow.Procurement.Application.Needs;

public interface INeedRepository
{
    Task AddAsync(Need need, CancellationToken cancellationToken);
}