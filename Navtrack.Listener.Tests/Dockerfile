FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS test
WORKDIR /src
COPY ["Navtrack.Listener/Navtrack.Listener.csproj", "Navtrack.Listener/"]
COPY ["Navtrack.Common/Navtrack.Common.csproj", "Navtrack.Common/"]
COPY ["Navtrack.DataAccess/Navtrack.DataAccess.csproj", "Navtrack.DataAccess/"]
COPY ["Navtrack.Library/Navtrack.Library.csproj", "Navtrack.Library/"]
COPY ["Navtrack.Listener.Tests/Navtrack.Listener.Tests.csproj", "Navtrack.Listener.Tests/"]
RUN dotnet restore "Navtrack.Listener.Tests/Navtrack.Listener.Tests.csproj"
COPY . .
WORKDIR /src/Navtrack.Listener.Tests
RUN dotnet test "Navtrack.Listener.Tests.csproj" 