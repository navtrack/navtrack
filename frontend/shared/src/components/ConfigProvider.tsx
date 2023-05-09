import { ReactNode } from "react";
import { useConfig } from "../hooks/app/config/useConfig";
import { useAxiosBaseUrls } from "../hooks/app/axios/useAxiosBaseUrls";
import { AppConfig } from "../state/appConfig";

type ConfigProviderProps = {
  children: ReactNode;
  config: AppConfig;
};

export function ConfigProvider(props: ConfigProviderProps) {
  const configSet = useConfig(props.config);
  const baseUrlsSet = useAxiosBaseUrls();

  return <>{configSet && baseUrlsSet && props.children}</>;
}
