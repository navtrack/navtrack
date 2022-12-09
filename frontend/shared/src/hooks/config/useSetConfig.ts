import { useEffect } from "react";
import { useRecoilState } from "recoil";
import { Config, configAtom } from "../../state/app.config";

export const useSetConfig = (config?: Config) => {
  const [state, setState] = useRecoilState(configAtom);

  useEffect(() => {
    if (state.config === undefined && !state.initialized) {
      setState({ config: config, initialized: true });
    }
  }, [config, setState, state]);

  return state.initialized;
};
