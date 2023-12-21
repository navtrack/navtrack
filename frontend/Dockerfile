FROM node:alpine AS build
WORKDIR /src
COPY . .
RUN npm install
RUN npm run build:web

FROM node:alpine
WORKDIR /app
COPY --from=build /src/web/build .
RUN npm install -g serve
EXPOSE 3000
CMD [ "serve", "/app" ]
