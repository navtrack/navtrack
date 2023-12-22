FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .

RUN dotnet restore
RUN dotnet test "backend/Navtrack.Listener.Tests/Navtrack.Listener.Tests.csproj"
RUN dotnet publish "backend/Navtrack.Listener/Navtrack.Listener.csproj" -c Release -o /app

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Navtrack.Listener.dll"]
