import { Fragment, ReactNode } from "react";
import { appConfigStore, AppConfig } from "../../state/appConfig";

type ConfigProviderProps = {
  children?: ReactNode;
  config: AppConfig;
};

export function ConfigProvider(props: ConfigProviderProps) {
  if (!appConfigStore.initialized) {
    appConfigStore.config = props.config;
    appConfigStore.initialized = true;
  }

  return <Fragment>{props.children}</Fragment>;
}
