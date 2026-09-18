using SupplyFlow.Procurement.Domain.Needs;

namespace SupplyFlow.Procurement.Application.Needs;

public sealed record NeedDto(
    Guid Id,
    string Description,
    decimal Quantity,
    DateOnly RequiredBy,
    NeedStatus Status);