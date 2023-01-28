FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS publish
WORKDIR /src
COPY . .
RUN dotnet tool restore
RUN dotnet publish "backend/Navtrack.Api/Navtrack.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Navtrack.Api.dll"]
