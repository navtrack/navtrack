docker build -t navtrack/api -f Navtrack.Api/Dockerfile .
docker build -t navtrack/web -f Navtrack.Web/Dockerfile .
docker build -t navtrack/listener -f Navtrack.Listener/Dockerfile .
