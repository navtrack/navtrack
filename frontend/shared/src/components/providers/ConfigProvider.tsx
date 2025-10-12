import { Fragment, ReactNode, useEffect } from "react";
import { AppConfig, appConfigAtom } from "../../state/appConfig";
import { useAtom } from "jotai";

type ConfigProviderProps = {
  children: ReactNode;
  config: AppConfig;
};

export function ConfigProvider(props: ConfigProviderProps) {
  const [state, setState] = useAtom(appConfigAtom);

  useEffect(() => {
    if (state === undefined) {
      setState(props.config);
    }
  }, [props.config, setState, state]);

  return <Fragment>{state !== undefined && props.children}</Fragment>;
}
