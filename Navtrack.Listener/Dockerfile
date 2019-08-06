FROM mcr.microsoft.com/dotnet/core/runtime:3.0 AS base
WORKDIR /app
EXPOSE 6801
EXPOSE 6802

FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src
COPY ["Navtrack.Listener/Navtrack.Listener.csproj", "Navtrack.Listener/"]
COPY ["Navtrack.Common/Navtrack.Common.csproj", "Navtrack.Common/"]
COPY ["Navtrack.DataAccess/Navtrack.DataAccess.csproj", "Navtrack.DataAccess/"]
COPY ["Navtrack.Library/Navtrack.Library.csproj", "Navtrack.Library/"]
RUN dotnet restore "Navtrack.Listener/Navtrack.Listener.csproj"
COPY . .
WORKDIR /src/Navtrack.Listener
RUN dotnet build "Navtrack.Listener.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Navtrack.Listener.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Navtrack.Listener.dll"]