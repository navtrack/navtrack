FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src
COPY ["Navtrack.Api/Navtrack.Api.csproj", "Navtrack.Api/"]
COPY ["Navtrack.Common/Navtrack.Common.csproj", "Navtrack.Common/"]
COPY ["Navtrack.DataAccess/Navtrack.DataAccess.csproj", "Navtrack.DataAccess/"]
COPY ["Navtrack.Library/Navtrack.Library.csproj", "Navtrack.Library/"]
RUN dotnet restore "Navtrack.Api/Navtrack.Api.csproj"
COPY . .
WORKDIR /src/Navtrack.Api
RUN dotnet build "Navtrack.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Navtrack.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Navtrack.Api.dll"]