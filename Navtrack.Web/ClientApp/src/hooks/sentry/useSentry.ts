import { init } from "@sentry/react";
import { Integrations } from "@sentry/tracing";
import { useEffect, useState } from "react";
import { useConfig } from "../config/useConfig";

export default function useSentry() {
  const config = useConfig();
  const [initalised, setInitialised] = useState(false);

  useEffect(() => {
    if (config?.sentryDsn && !initalised) {
      setInitialised(true);
      init({
        dsn: config.sentryDsn,
        integrations: [new Integrations.BrowserTracing()],
        environment: config.environment
      });
    }
  }, [config?.environment, config?.sentryDsn, initalised]);
}
