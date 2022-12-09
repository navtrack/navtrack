import { useEffect } from "react";
import { useRecoilState } from "recoil";
import { settingsAtom } from "../../state/app.settings";
import { useSettingsQuery } from "../queries/useSettingsQuery";

export const useSettings = () => {
  const settings = useSettingsQuery();
  const [state, setState] = useRecoilState(settingsAtom);

  useEffect(() => {
    if (!state.initialized) {
      if (settings.data) {
        setState({ settings: settings.data, initialized: true });
      }
    }
  }, [setState, settings.data, state.initialized]);

  return state.initialized;
};
