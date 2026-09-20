FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/SupplyFlow.Supplier.Api/SupplyFlow.Supplier.Api.csproj", "src/SupplyFlow.Supplier.Api/"]
COPY ["src/SupplyFlow.Supplier.Application/SupplyFlow.Supplier.Application.csproj", "src/SupplyFlow.Supplier.Application/"]
COPY ["src/SupplyFlow.Supplier.Domain/SupplyFlow.Supplier.Domain.csproj", "src/SupplyFlow.Supplier.Domain/"]
COPY ["src/SupplyFlow.Supplier.Infrastructure/SupplyFlow.Supplier.Infrastructure.csproj", "src/SupplyFlow.Supplier.Infrastructure/"]
COPY ["src/SupplyFlow.Contracts/SupplyFlow.Contracts.csproj", "src/SupplyFlow.Contracts/"]

RUN dotnet restore "src/SupplyFlow.Supplier.Api/SupplyFlow.Supplier.Api.csproj"

COPY src/ src/

RUN dotnet publish \
    "src/SupplyFlow.Supplier.Api/SupplyFlow.Supplier.Api.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 5002

ENTRYPOINT ["dotnet", "SupplyFlow.Supplier.Api.dll"]