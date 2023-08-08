import { ReactNode, useEffect } from "react";
import { AppConfig, appConfigAtom } from "../state/appConfig";
import { useRecoilState } from "recoil";

type ConfigProviderProps = {
  children: ReactNode;
  config: AppConfig;
};

export function ConfigProvider(props: ConfigProviderProps) {
  const [state, setState] = useRecoilState(appConfigAtom);

  useEffect(() => {
    if (state === undefined) {
      setState(props.config);
    }
  }, [props.config, setState, state]);

  return <>{state !== undefined && props.children}</>;
}
