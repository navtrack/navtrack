import { IntlProvider } from "react-intl";
import { QueryClient, QueryClientProvider } from "react-query";
import { RecoilRoot } from "recoil";
import { SentryProvider } from "./SentryProvider";
import { AppConfig } from "@navtrack/shared/state/appConfig";
import { AxiosConfigurator } from "@navtrack/shared/components/AxiosConfigurator";
import { ConfigProvider } from "@navtrack/shared/components/ConfigProvider";
import { ReactNode, Suspense } from "react";
import { BrowserRouterProvider } from "./BrowserRouterProvider";
import { Authentication } from "@navtrack/shared/components/Authentication";

const queryClient = new QueryClient();

type BaseAppProps = {
  privateRoutes: ReactNode;
  publicRoutes: ReactNode;
  translations: Record<string, string>;
  config: AppConfig;
};

export function BaseApp(props: BaseAppProps) {
  return (
    <Suspense>
      <RecoilRoot>
        <QueryClientProvider client={queryClient}>
          <ConfigProvider config={props.config}>
            <SentryProvider>
              <AxiosConfigurator>
                <IntlProvider locale="en" messages={props.translations}>
                  <Authentication>
                    <BrowserRouterProvider
                      privateRoutes={props.privateRoutes}
                      publicRoutes={props.publicRoutes}
                    />
                  </Authentication>
                </IntlProvider>
              </AxiosConfigurator>
            </SentryProvider>
          </ConfigProvider>
        </QueryClientProvider>
      </RecoilRoot>
    </Suspense>
  );
}
