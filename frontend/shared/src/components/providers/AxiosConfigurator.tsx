import { Fragment, ReactNode } from "react";
import { useAxiosAuthorization } from "../../hooks/app/axios/useAxiosAuthorization";
import { useAxiosBaseUrls } from "../../hooks/app/axios/useAxiosBaseUrls";

type AxiosConfiguratorProps = {
  children: ReactNode;
};

export function AxiosConfigurator(props: AxiosConfiguratorProps) {
  const baseUrlsSet = useAxiosBaseUrls();
  const configured = useAxiosAuthorization();

  return <Fragment>{baseUrlsSet && configured && props.children}</Fragment>;
}
