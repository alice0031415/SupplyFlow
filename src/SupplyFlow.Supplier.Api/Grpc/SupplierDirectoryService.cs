using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using SupplyFlow.Contracts.Grpc;
using SupplyFlow.Supplier.Infrastructure.Persistence;

namespace SupplyFlow.Supplier.Api.Grpc;

public sealed class SupplierDirectoryService(
    SupplyFlowSupplierDbContext dbContext)
    : SupplierDirectory.SupplierDirectoryBase
{
    public override async Task<GetTenderStatusResponse> GetTenderStatus(
        GetTenderStatusRequest request,
        ServerCallContext context)
    {
        if (!Guid.TryParse(request.NeedId, out var needId))
        {
            throw new RpcException(
                new Status(
                    StatusCode.InvalidArgument,
                    "Invalid NeedId."));
        }

        var tender = await dbContext.ReceivedTenders
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.NeedId == needId,
                context.CancellationToken);

        return tender is null
            ? new GetTenderStatusResponse
            {
                NeedId = needId.ToString(),
                Received = false
            }
            : new GetTenderStatusResponse
            {
                NeedId = needId.ToString(),
                Received = true,
                ReceivedAtUtc = tender.ReceivedAtUtc.ToString("O")
            };
    }
}