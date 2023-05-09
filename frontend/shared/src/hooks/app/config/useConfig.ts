import { useEffect } from "react";
import { useRecoilState } from "recoil";
import { AppConfig, appConfigAtom } from "../../../state/appConfig";

export const useConfig = (config?: AppConfig) => {
  const [state, setState] = useRecoilState(appConfigAtom);

  useEffect(() => {
    if (state === undefined) {
      setState(config);
    }
  }, [config, setState, state]);

  return !!state;
};
