docker build -t navtrack/api --platform linux/amd64,linux/arm64 -f backend/Navtrack.Api/Dockerfile .
docker build -t navtrack/web --platform linux/amd64,linux/arm64 -f frontend/Dockerfile frontend
docker build -t navtrack/listener --platform linux/amd64,linux/arm64 -f backend/Navtrack.Listener/Dockerfile .
