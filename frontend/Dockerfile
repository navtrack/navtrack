FROM --platform=$BUILDPLATFORM node:alpine AS build
WORKDIR /src
COPY . .
RUN npm install
RUN npm run build

FROM --platform=$BUILDPLATFORM node:alpine
WORKDIR /app
COPY --from=build /src/frontend/web/build .
COPY --from=build /src/frontend/run_web.sh /run_web.sh
RUN chmod +x /run_web.sh
RUN npm install -g serve
EXPOSE 3000
CMD ["/run_web.sh"]
