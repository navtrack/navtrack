FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src
COPY ["Navtrack.Api.csproj", "Navtrack.Api/"]
RUN dotnet restore "Navtrack.Api/Navtrack.Api.csproj"
COPY . Navtrack.Api/
WORKDIR /src/Navtrack.Api
RUN dotnet build "Navtrack.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Navtrack.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Navtrack.Api.dll"]