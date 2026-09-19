using MediatR;

namespace SupplyFlow.Procurement.Application.Suppliers;

public sealed record GetTenderStatusQuery(
    Guid NeedId) : IRequest<SupplierTenderStatus?>;

public sealed class GetTenderStatusHandler(
    ISupplierGateway supplierGateway)
    : IRequestHandler<GetTenderStatusQuery, SupplierTenderStatus?>
{
    public async Task<SupplierTenderStatus?> Handle(
        GetTenderStatusQuery request,
        CancellationToken cancellationToken)
    {
        return await supplierGateway.GetTenderStatusAsync(
            request.NeedId,
            cancellationToken);
    }
}