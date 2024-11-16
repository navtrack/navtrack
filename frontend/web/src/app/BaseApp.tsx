import { IntlProvider } from "react-intl";
import { RecoilRoot } from "recoil";
import { AppConfig } from "@navtrack/shared/state/appConfig";
import { AxiosConfigurator } from "@navtrack/shared/components/providers/AxiosConfigurator";
import { ConfigProvider } from "@navtrack/shared/components/providers/ConfigProvider";
import { ReactNode, Suspense } from "react";
import { BrowserRouterProvider } from "./BrowserRouterProvider";
import { AuthenticationProvider } from "@navtrack/shared/components/providers/AuthenticationProvider";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";

const queryClient = new QueryClient();

type BaseAppProps = {
  privateRoutes: ReactNode;
  publicRoutes: ReactNode;
  translations: Record<string, string>;
  config: AppConfig;
  slotProvider: (props: { children: ReactNode }) => ReactNode;
};

export function BaseApp(props: BaseAppProps) {
  return (
    <Suspense>
      <RecoilRoot>
        <QueryClientProvider client={queryClient}>
          {props.config.reactQueryDevtools && <ReactQueryDevtools />}
          <ConfigProvider config={props.config}>
            <AxiosConfigurator>
              <IntlProvider locale="en" messages={props.translations}>
                <AuthenticationProvider>
                  {props.slotProvider({
                    children: (
                      <BrowserRouterProvider
                        privateRoutes={props.privateRoutes}
                        publicRoutes={props.publicRoutes}
                      />
                    )
                  })}
                </AuthenticationProvider>
              </IntlProvider>
            </AxiosConfigurator>
          </ConfigProvider>
        </QueryClientProvider>
      </RecoilRoot>
    </Suspense>
  );
}
