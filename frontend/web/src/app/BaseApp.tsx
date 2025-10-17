import { IntlProvider } from "react-intl";
import { Provider } from "jotai";
import { AppConfig } from "@navtrack/shared/state/appConfig";
import { AxiosConfigurator } from "@navtrack/shared/components/providers/AxiosConfigurator";
import { ConfigProvider } from "@navtrack/shared/components/providers/ConfigProvider";
import { ReactNode, Suspense } from "react";
import { BrowserRouterProvider } from "./BrowserRouterProvider";
import { AuthenticationProvider } from "@navtrack/shared/components/providers/AuthenticationProvider";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";
import { ThemeInit } from "../../.flowbite-react/init";

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
      <Provider>
        <ThemeInit />
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
      </Provider>
    </Suspense>
  );
}
