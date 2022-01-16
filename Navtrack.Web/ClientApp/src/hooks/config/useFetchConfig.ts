import axios from "axios";
import { useEffect } from "react";
import { useRecoilState } from "recoil";
import { LocalConfig } from "../../app.config";
import { configAtom } from "../../state/app.config";
import { isDevEnv } from "../../utils/isDevEnv";

export const useFetchConfig = () => {
  const [state, setState] = useRecoilState(configAtom);

  useEffect(() => {
    if (state.config === undefined && !state.initialized) {
      if (isDevEnv) {
        setState({ config: LocalConfig, initialized: true });
      } else {
        axios({
          url: "/config.json"
        }).then((response) => {
          setState({ config: response.data, initialized: true });
        });
      }
    }
  }, [setState, state]);

  return state.initialized;
};
