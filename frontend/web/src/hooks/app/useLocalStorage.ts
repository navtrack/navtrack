import {
  localStorageAtom,
  LocalStorageData
} from "@navtrack/ui-shared/state/app.localStorage";
import { useEffect } from "react";
import { useRecoilState } from "recoil";

export const CONTEXT_KEY = "navtrack";

export default function useLocalStorage() {
  const [state, setState] = useRecoilState(localStorageAtom);

  useEffect(() => {
    if (!state.initialized) {
      const json = localStorage.getItem(CONTEXT_KEY);

      if (json) {
        try {
          const data = JSON.parse(json) as LocalStorageData;

          setState({ initialized: true, data });
        } catch (e) {
          setState({ initialized: true });
        }
      } else {
        setState({ initialized: true });
      }
    }
  }, [setState, state.initialized]);

  useEffect(() => {
    if (state.initialized) {
      localStorage.setItem(CONTEXT_KEY, JSON.stringify(state.data));
    }
  }, [state.data, state.initialized]);

  return state.initialized;
}
