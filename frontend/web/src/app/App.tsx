import { IntlProvider } from "react-intl";
import translations from "../translations";
import { BrowserRouterProvider } from "./BrowserRouterProvider";
import { QueryClient, QueryClientProvider } from "react-query";
import { RecoilRoot } from "recoil";
import ConfigProvider from "./ConfigProvider";
import { SentryProvider } from "./SentryProvider";
import { AuthenticationProvider } from "./AuthenticationProvider";
import { SettingsProvider } from "./SettingsProvider";

const queryClient = new QueryClient();

export function App() {
  return (
    <RecoilRoot>
      <QueryClientProvider client={queryClient}>
        <ConfigProvider>
          <SettingsProvider>
            <SentryProvider>
              <AuthenticationProvider>
                <IntlProvider locale="en" messages={translations["en"]}>
                  <BrowserRouterProvider />
                </IntlProvider>
              </AuthenticationProvider>
            </SentryProvider>
          </SettingsProvider>
        </ConfigProvider>
      </QueryClientProvider>
    </RecoilRoot>
  );
}
