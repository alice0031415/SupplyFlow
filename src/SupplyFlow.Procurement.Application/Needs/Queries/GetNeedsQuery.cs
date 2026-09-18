using MediatR;

namespace SupplyFlow.Procurement.Application.Needs.Queries;

public sealed record GetNeedsQuery
    : IRequest<IReadOnlyList<NeedDto>>;

public sealed class GetNeedsHandler(
    INeedRepository repository)
    : IRequestHandler<GetNeedsQuery, IReadOnlyList<NeedDto>>
{
    public Task<IReadOnlyList<NeedDto>> Handle(
        GetNeedsQuery request,
        CancellationToken cancellationToken)
    {
        return repository.GetAllAsync(cancellationToken);
    }
}