## Navtrack
### Open source GPS tracking system based on .NET and React.
[navtrack.com](https://navtrack.com) - [codeagency.com](https://codeagency.com)

[![Discord](https://img.shields.io/discord/515183168060391427?label=Discord)](https://discord.gg/N4ZxhB3y6h)

## Running Navtrack

1. [From source code](#running-from-source-code)
2. [With Docker containers](#running-with-docker-containers)


## Running from source code

#### Requirements

- .NET 7 SDK (https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
- Node.js (https://nodejs.org/)
- Docker (to run MongoDB) (https://docs.docker.com/get-docker/)
- Optional: Rider or Visual Studio


#### Steps

1. Clone this repository
2. Open up a terminal
3. Build the solution with `dotnet build`
4. Start MongoDB using our development docker-compose file or if you already have a MongoDB instance running you can configure the connection strings in the following 2 files, one for the API and one for the Listener service.

```
docker-compose -f docker-compose.dev.yml up
```

```
backend/Navtrack.Api/appsettings.Development.json
backend/Navtrack.Listener/appsettings.Development.json
```
5. Start the API with `dotnet run --project Navtrack.Api`
6. Start the Listener service with `dotnet run --project Navtrack.Listener`
7. Start the web interface with `cd frontend/web && npm start`

## Running with Docker containers

#### 1. Install docker

#### 2. Get the latest docker-compose.yml
```
curl https://raw.githubusercontent.com/navtrack/navtrack/main/docker-compose.yml -o docker-compose.yml
```

#### 3. Run Docker Compose 
```
docker-compose up
```

#### 4. Open Navtrack in a browser
[http://localhost:3000](http://localhost:3000)
