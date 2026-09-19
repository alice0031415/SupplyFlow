using SupplyFlow.Contracts.Grpc;
using SupplyFlow.Procurement.Application.Suppliers;

namespace SupplyFlow.Procurement.Infrastructure.Grpc;

public sealed class SupplierGateway(
    SupplierDirectory.SupplierDirectoryClient client)
    : ISupplierGateway
{
    public async Task<SupplierTenderStatus> GetTenderStatusAsync(
        Guid needId,
        CancellationToken cancellationToken)
    {
        var response = await client.GetTenderStatusAsync(
            new GetTenderStatusRequest
            {
                NeedId = needId.ToString()
            },
            cancellationToken: cancellationToken);

        DateTimeOffset? receivedAt = null;

        if (response.Received &&
            DateTimeOffset.TryParse(
                response.ReceivedAtUtc,
                out var parsed))
        {
            receivedAt = parsed;
        }

        return new SupplierTenderStatus(
            needId,
            response.Received,
            receivedAt);
    }
}