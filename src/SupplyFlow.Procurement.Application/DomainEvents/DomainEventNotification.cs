using MediatR;
using SupplyFlow.Procurement.Domain.Common;

namespace SupplyFlow.Procurement.Application.DomainEvents;

public sealed record DomainEventNotification<TDomainEvent>(
    TDomainEvent DomainEvent) : INotification
    where TDomainEvent : IDomainEvent;