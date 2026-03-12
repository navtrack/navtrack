import { Fragment, ReactNode } from "react";
import { appConfigStore, AppConfig } from "../../state/appConfig";

type ConfigProviderProps = {
  children?: ReactNode;
  config: AppConfig;
};

export function ConfigProvider(props: ConfigProviderProps) {
  if (appConfigStore.config === undefined) {
    appConfigStore.config = props.config;
  }

  return <Fragment>{props.children}</Fragment>;
}
