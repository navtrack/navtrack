import { environmentSettingsSelector } from "@navtrack/shared/state/environment";
import { init } from "@sentry/react";
import { Integrations } from "@sentry/tracing";
import { useEffect, useState } from "react";
import { useRecoilValue } from "recoil";

export function useSentry() {
  const settings = useRecoilValue(environmentSettingsSelector);
  const [initalised, setInitialised] = useState(false);

  useEffect(() => {
    if (settings["Sentry.Dsn"] && !initalised) {
      setInitialised(true);
      init({
        dsn: settings["Sentry.Dsn"],
        integrations: [new Integrations.BrowserTracing()],
        environment: settings["Application.Environment"]
      });
    }
  }, [initalised, settings]);
}
