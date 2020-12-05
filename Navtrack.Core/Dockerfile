FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS publish
WORKDIR /src
COPY . .
RUN curl -sL https://deb.nodesource.com/setup_14.x | bash -
RUN apt-get install -y nodejs
RUN dotnet publish "Navtrack.Core/Navtrack.Core.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Navtrack.Core.dll"]