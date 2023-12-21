docker buildx build -t navtrack/api --platform linux/amd64,linux/arm64 -f backend/Navtrack.Api/Dockerfile .
docker buildx build -t navtrack/web --platform linux/amd64,linux/arm64 -f frontend/Dockerfile frontend
docker buildx build -t navtrack/listener --platform linux/amd64,linux/arm64 -f backend/Navtrack.Listener/Dockerfile .
