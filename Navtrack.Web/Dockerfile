FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src
COPY ["Navtrack.Web/Navtrack.Web.csproj", "Navtrack.Web/"]
COPY ["Navtrack.Common/Navtrack.Common.csproj", "Navtrack.Common/"]
COPY ["Navtrack.DataAccess/Navtrack.DataAccess.csproj", "Navtrack.DataAccess/"]
COPY ["Navtrack.Library/Navtrack.Library.csproj", "Navtrack.Library/"]
RUN dotnet restore "Navtrack.Web/Navtrack.Web.csproj"
COPY . .
WORKDIR /src/Navtrack.Web
RUN dotnet build "Navtrack.Web.csproj" -c Release -o /app

FROM build AS publish
RUN curl -sL https://deb.nodesource.com/setup_13.x | bash -
RUN apt-get install -y nodejs
RUN dotnet publish "Navtrack.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Navtrack.Web.dll"]