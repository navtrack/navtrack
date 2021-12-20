import { useCallback, useEffect } from "react";
import { useRecoilState } from "recoil";
import { AppContext, appContextAtom } from "../../state/appContext";

const CONTEXT_KEY = "navtrack.appContext";

export default function useAppContext() {
  const [appContextState, setAppContextState] = useRecoilState(appContextAtom);

  const setAppContext = useCallback(
    (appContextFunc: (current: AppContext) => AppContext) => {
      const newAppContext: AppContext = appContextFunc(appContextState);
      setAppContextState(newAppContext);
      localStorage.setItem(CONTEXT_KEY, JSON.stringify(newAppContext));
    },
    [appContextState, setAppContextState]
  );

  useEffect(() => {
    if (!appContextState.initialized) {
      const appContextFromStorage = localStorage.getItem(CONTEXT_KEY);

      if (appContextFromStorage) {
        const deserialized = JSON.parse(appContextFromStorage) as AppContext;
        setAppContext(() => ({
          ...deserialized,
          token: deserialized.token
            ? {
                ...deserialized.token,
                expiryDate: new Date(deserialized.token.expiryDate)
              }
            : undefined
        }));
      } else {
        setAppContext(() => ({ initialized: true }));
      }
    }
  }, [appContextState, setAppContext, setAppContextState]);

  return { appContext: appContextState, setAppContext };
}
