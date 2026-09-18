using MediatR;
using SupplyFlow.Procurement.Domain.Needs;

namespace SupplyFlow.Procurement.Application.Needs.Commands;

public sealed record CreateNeedCommand(
    string Description,
    decimal Quantity,
    DateOnly RequiredBy) : IRequest<Guid>;

public sealed class CreateNeedHandler(
    INeedRepository repository) : IRequestHandler<CreateNeedCommand, Guid>
{
    public async Task<Guid> Handle(
        CreateNeedCommand request,
        CancellationToken cancellationToken)
    {
        var need = new Need(
            request.Description,
            request.Quantity,
            request.RequiredBy);

        await repository.AddAsync(need, cancellationToken);

        return need.Id;
    }
}