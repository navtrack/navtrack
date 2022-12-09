import { useRecoilValue } from "recoil";
import { PublicClientApplication } from "@azure/msal-browser";
import { useMemo } from "react";
import { settingsSelector } from "@navtrack/ui-shared/state/app.settings";

export default function useMicrosoftLogin() {
  const settings = useRecoilValue(settingsSelector);

  const publicClientApplication = useMemo(
    () =>
      !!settings["MicrosoftAuthentication.ClientId"] &&
      !!settings["MicrosoftAuthentication.Authority"]
        ? new PublicClientApplication({
            auth: {
              clientId: settings["MicrosoftAuthentication.ClientId"],
              authority: settings["MicrosoftAuthentication.Authority"]
            },
            cache: {
              cacheLocation: "sessionStorage"
            }
          })
        : undefined,
    [settings]
  );

  return { publicClientApplication };
}
