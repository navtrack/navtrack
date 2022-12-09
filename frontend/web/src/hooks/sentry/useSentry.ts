import { settingsSelector } from "@navtrack/ui-shared/state/app.settings";
import { init } from "@sentry/react";
import { Integrations } from "@sentry/tracing";
import { useEffect, useState } from "react";
import { useRecoilValue } from "recoil";

export default function useSentry() {
  const settings = useRecoilValue(settingsSelector);
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
