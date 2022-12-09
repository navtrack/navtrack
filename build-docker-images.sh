docker build -t navtrack/api -f backend/Navtrack.Api/Dockerfile .
# docker build -t navtrack/web -f Navtrack.Web/Dockerfile .
docker build -t navtrack/listener -f backend/Navtrack.Listener/Dockerfile .
