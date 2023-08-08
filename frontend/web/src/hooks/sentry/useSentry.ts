import { appConfigAtom } from "@navtrack/shared/state/appConfig";
import { init } from "@sentry/react";
import { Integrations } from "@sentry/tracing";
import { useEffect, useState } from "react";
import { useRecoilValue } from "recoil";

export function useSentry() {
  const appConfig = useRecoilValue(appConfigAtom);
  const [initalised, setInitialised] = useState(false);

  useEffect(() => {
    if (appConfig?.sentry?.dsn && !initalised) {
      setInitialised(true);
      init({
        dsn: appConfig?.sentry.dsn,
        integrations: [new Integrations.BrowserTracing()]
        // environment: appConfig["Application.Environment"] TODO
      });
    }
  }, [initalised, appConfig]);
}
