FROM mcr.microsoft.com/dotnet/sdk:7.0 AS publish
WORKDIR /src
COPY . .
RUN curl -sL https://deb.nodesource.com/setup_19.x | bash -
RUN apt-get install -y nodejs
RUN dotnet publish "backend/Navtrack.Web/Navtrack.Web.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Navtrack.Web.dll"]
