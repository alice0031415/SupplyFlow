FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/SupplyFlow.Procurement.Api/SupplyFlow.Procurement.Api.csproj", "src/SupplyFlow.Procurement.Api/"]
COPY ["src/SupplyFlow.Procurement.Application/SupplyFlow.Procurement.Application.csproj", "src/SupplyFlow.Procurement.Application/"]
COPY ["src/SupplyFlow.Procurement.Domain/SupplyFlow.Procurement.Domain.csproj", "src/SupplyFlow.Procurement.Domain/"]
COPY ["src/SupplyFlow.Procurement.Infrastructure/SupplyFlow.Procurement.Infrastructure.csproj", "src/SupplyFlow.Procurement.Infrastructure/"]
COPY ["src/SupplyFlow.Contracts/SupplyFlow.Contracts.csproj", "src/SupplyFlow.Contracts/"]

RUN dotnet restore "src/SupplyFlow.Procurement.Api/SupplyFlow.Procurement.Api.csproj"

COPY src/ src/

RUN dotnet publish \
    "src/SupplyFlow.Procurement.Api/SupplyFlow.Procurement.Api.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "SupplyFlow.Procurement.Api.dll"]