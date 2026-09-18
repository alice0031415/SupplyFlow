using MediatR;

namespace SupplyFlow.Procurement.Application.Needs.Commands;

public sealed record PublishTenderCommand(
    Guid NeedId) : IRequest<bool>;

public sealed class PublishTenderHandler(
    INeedRepository repository)
    : IRequestHandler<PublishTenderCommand, bool>
{
    public async Task<bool> Handle(
        PublishTenderCommand request,
        CancellationToken cancellationToken)
    {
        var need = await repository.GetByIdAsync(
            request.NeedId,
            cancellationToken);

        if (need is null)
            return false;

        need.PublishTender();

        await repository.SaveChangesAsync(
            cancellationToken);

        return true;
    }
}