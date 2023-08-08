import { IntlProvider } from "react-intl";
import { BrowserRouterProvider } from "./BrowserRouterProvider";
import { QueryClient, QueryClientProvider } from "react-query";
import { RecoilRoot } from "recoil";
import { SentryProvider } from "./SentryProvider";
import { translations } from "@navtrack/shared/translations";
import { AUTHENTICATION } from "../constants";
import { AppConfig } from "@navtrack/shared/state/appConfig";
import { AxiosConfigurator } from "@navtrack/shared/components/AxiosConfigurator";
import { ConfigProvider } from "@navtrack/shared/components/ConfigProvider";

const queryClient = new QueryClient();
const config: AppConfig = {
  api: {
    url: `${import.meta.env.VITE_API_URL}`
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
    <RecoilRoot>
      <QueryClientProvider client={queryClient}>
        <ConfigProvider config={config}>
          <SentryProvider>
            <AxiosConfigurator>
              <IntlProvider locale="en" messages={translations["en"]}>
                <BrowserRouterProvider />
              </IntlProvider>
            </AxiosConfigurator>
          </SentryProvider>
        </ConfigProvider>
      </QueryClientProvider>
    </RecoilRoot>
  );
}
