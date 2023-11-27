import { translations } from "@navtrack/shared/translations";
import { AUTHENTICATION } from "../constants";
import { AppConfig } from "@navtrack/shared/state/appConfig";
import { BaseApp } from "./BaseApp";
import { AuthenticatedRoutes } from "./AuthenticatedRoutes";
import { UnauthenticatedRoutes } from "./UnauthenticatedRoutes";
import { SlotProvider } from "./SlotProvider";

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
      publicRoutes={<UnauthenticatedRoutes />}
      privateRoutes={<AuthenticatedRoutes />}
      config={config}
      translations={translations["en"]}
      slotProvider={(props) => <SlotProvider {...props} />}
    />
  );
}
