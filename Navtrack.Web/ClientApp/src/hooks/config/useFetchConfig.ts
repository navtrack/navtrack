import axios from "axios";
import { useEffect } from "react";
import { useRecoilState } from "recoil";
import { LocalConfig } from "../../app.config";
import { isDevEnv } from "../../utils/isDevEnv";
import { configState } from "./state";

export const useFetchConfig = () => {
  const [state, setState] = useRecoilState(configState);

  useEffect(() => {
    if (state.config === undefined && !state.loading) {
      if (isDevEnv) {
        setState({ config: LocalConfig });
      } else {
        setState({ loading: true });
        axios({
          url: "/config.json"
        }).then((response) => {
          setState({ config: response.data });
        });
      }
    }
  }, [setState, state]);

  return state.config !== undefined;
};
