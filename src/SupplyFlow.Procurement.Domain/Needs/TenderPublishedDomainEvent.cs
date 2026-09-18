using SupplyFlow.Procurement.Domain.Common;

namespace SupplyFlow.Procurement.Domain.Needs;

public sealed record TenderPublishedDomainEvent(
    Guid NeedId) : IDomainEvent;