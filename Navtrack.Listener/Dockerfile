FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Navtrack.Listener/Navtrack.Listener.csproj", "Navtrack.Listener/"]
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