FROM mcr.microsoft.com/dotnet/sdk:5.0 AS builder
WORKDIR /app
COPY . .
RUN dotnet test
RUN dotnet publish -c Release -o package

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=builder /app/package .
ENTRYPOINT ["dotnet", "car-stocks.dll"]