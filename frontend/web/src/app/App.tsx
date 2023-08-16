import { translations } from "@navtrack/shared/translations";
import { AUTHENTICATION } from "../constants";
import { AppConfig } from "@navtrack/shared/state/appConfig";
import { BaseApp } from "./BaseApp";
import { RoutesUnauthenticated } from "./RoutesUnauthenticated";
import { RoutesAuthenticated } from "./RoutesAuthenticated";

const config: AppConfig = {
  api: {
    url: import.meta.env.VITE_API_URL
  },
  map: {
    tileUrl: import.meta.env.VITE_MAP_TILE_URL
  },
  authentication: {
    clientId: AUTHENTICATION.CLIENT_ID
  }
};

export function App() {
  return (
    <BaseApp
      publicRoutes={<RoutesUnauthenticated />}
      privateRoutes={<RoutesAuthenticated />}
      config={config}
      translations={translations["en"]}
    />
  );
}
