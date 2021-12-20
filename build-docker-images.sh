docker build -t navtrack/api:latest -f Navtrack.Api/Dockerfile .
docker build -t navtrack/web:latest -f Navtrack.Web/Dockerfile .
docker build -t navtrack/listener:latest -f  Navtrack.Listener/Dockerfile .
