import { IntlProvider } from "react-intl";
import translations from "../translations";
import BrowserRouterProvider from "./BrowserRouterProvider";
import { QueryClient, QueryClientProvider } from "react-query";
import { RecoilRoot } from "recoil";
import ConfigProvider from "./ConfigProvider";
import SentryProvider from "./SentryProvider";
import AppContextProvider from "./AppContextProvider";
import AxiosConfigurationProvider from "./AxiosConfigurationProvider";
import TokenRefreshProvider from "./TokenRefreshProvider";

const queryClient = new QueryClient();

export function App() {
  return (
    <RecoilRoot>
      <ConfigProvider>
        <SentryProvider>
          <AppContextProvider>
            <QueryClientProvider client={queryClient}>
              <AxiosConfigurationProvider>
                <TokenRefreshProvider>
                  <IntlProvider locale="en" messages={translations["en"]}>
                    <BrowserRouterProvider />
                  </IntlProvider>
                </TokenRefreshProvider>
              </AxiosConfigurationProvider>
            </QueryClientProvider>
          </AppContextProvider>
        </SentryProvider>
      </ConfigProvider>
    </RecoilRoot>
  );
}
