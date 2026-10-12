import { IntlProvider } from "react-intl";
import { Provider } from "jotai";
import { AppConfig } from "@navtrack/shared/state/appConfig";
import { AxiosConfigurator } from "@navtrack/shared/components/providers/AxiosConfigurator";
import { ConfigProvider } from "@navtrack/shared/components/providers/ConfigProvider";
import { ReactNode } from "react";
import { BrowserRouterProvider } from "./BrowserRouterProvider";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";
import { AppSlots, SlotContext } from "./SlotContext";
import { jotaiStore } from "@navtrack/shared/state/store";

const queryClient = new QueryClient();

type BaseAppProps = {
  privateRoutes: ReactNode;
  publicRoutes: ReactNode;
  translations: Record<string, string>;
  config: AppConfig;
  slots?: AppSlots;
};

export function BaseApp(props: BaseAppProps) {
  return (
    <ConfigProvider config={props.config}>
      <Provider store={jotaiStore}>
        <QueryClientProvider client={queryClient}>
          <AxiosConfigurator>
            {props.config.reactQueryDevtools && <ReactQueryDevtools />}
            <IntlProvider locale="en" messages={props.translations}>
              <SlotContext.Provider value={props.slots}>
                <BrowserRouterProvider
                  privateRoutes={props.privateRoutes}
                  publicRoutes={props.publicRoutes}
                />
              </SlotContext.Provider>
            </IntlProvider>
          </AxiosConfigurator>
        </QueryClientProvider>
      </Provider>
    </ConfigProvider>
  );
}
