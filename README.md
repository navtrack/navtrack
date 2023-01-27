## Navtrack
### Open source GPS tracking system based on .NET and React.
[navtrack.com](https://navtrack.com) - [codeagency.com](https://codeagency.com)

[![Discord](https://img.shields.io/discord/515183168060391427?label=Discord)](https://discord.gg/N4ZxhB3y6h)

## Running Navtrack

### From source code

#### Requirements

1. Node.js (https://nodejs.org/)
2. Docker (to run MongoDB) (https://docs.docker.com/get-docker/)
3. Or an instance of MongoDB running


#### Steps

1. Clone this repository
2. Start MongoDB using our development docker-compose file.

```
docker-compose -f docker-compose.dev.yml up
```

Or if you have a MongoDB instance already running you can configure the connection strings in 2 files, one for the API and one for the Listener service.

```
backend/Navtrack.Api/appsettings.Development.json
backend/Navtrack.Listener/appsettings.Development.json
```

3. Open up `Navtrack.sln` in Visual Studio or Rider
4. Build the solution
5. Start `Navtrack.Api`
6. Start `Navtrack.Listener`
7. Open up a terminal and go to the `frontend` directory
8. Run `npm install`
9. Go into the `web` directory
10. Run `npm start`

### With containers

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
