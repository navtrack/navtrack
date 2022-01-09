import { IntlProvider } from "react-intl";
import translations from "../translations";
import BrowserRouterProvider from "./BrowserRouterProvider";
import { QueryClient, QueryClientProvider } from "react-query";
import { RecoilRoot } from "recoil";
import ConfigProvider from "./ConfigProvider";
import SentryProvider from "./SentryProvider";
import AxiosConfigurationProvider from "./AxiosConfigurationProvider";
import AuthenticationProvider from "./AuthenticationProvider";

const queryClient = new QueryClient();

export function App() {
  return (
    <RecoilRoot>
      <ConfigProvider>
        <SentryProvider>
          <QueryClientProvider client={queryClient}>
            <AxiosConfigurationProvider>
              <AuthenticationProvider>
                <IntlProvider locale="en" messages={translations["en"]}>
                  <BrowserRouterProvider />
                </IntlProvider>
              </AuthenticationProvider>
            </AxiosConfigurationProvider>
          </QueryClientProvider>
        </SentryProvider>
      </ConfigProvider>
    </RecoilRoot>
  );
}
