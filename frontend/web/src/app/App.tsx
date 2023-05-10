import { IntlProvider } from "react-intl";
import { BrowserRouterProvider } from "./BrowserRouterProvider";
import { QueryClient, QueryClientProvider } from "react-query";
import { RecoilRoot } from "recoil";
import { SentryProvider } from "./SentryProvider";
import { translations } from "@navtrack/shared/translations";
import { ConfigProvider } from "@navtrack/shared/components/ConfigProvider";
import { AuthenticationProvider } from "@navtrack/shared/components/AuthenticationProvider";
import { AUTHENTICATION } from "../constants";
import { SignalRProvider } from "@navtrack/shared/components/SignalRProvider";
import { EnvironmentFetcher } from "@navtrack/shared/components/EnvironmentFetcher";

const queryClient = new QueryClient();
const config = {
  apiUrl: `${process.env.REACT_APP_API_URL}`
};

export function App() {
  return (
    <RecoilRoot>
      <QueryClientProvider client={queryClient}>
        <ConfigProvider config={config}>
          <EnvironmentFetcher>
            <SentryProvider>
              <AuthenticationProvider clientId={AUTHENTICATION.CLIENT_ID}>
                <SignalRProvider>
                  <IntlProvider locale="en" messages={translations["en"]}>
                    <BrowserRouterProvider />
                  </IntlProvider>
                </SignalRProvider>
              </AuthenticationProvider>
            </SentryProvider>
          </EnvironmentFetcher>
        </ConfigProvider>
      </QueryClientProvider>
    </RecoilRoot>
  );
}
