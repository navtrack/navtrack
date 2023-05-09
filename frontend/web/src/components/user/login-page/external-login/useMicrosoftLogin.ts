import { useRecoilValue } from "recoil";
import { PublicClientApplication } from "@azure/msal-browser";
import { useMemo } from "react";
import { environmentSettingsSelector } from "@navtrack/shared/state/environment";

export function useMicrosoftLogin() {
  const settings = useRecoilValue(environmentSettingsSelector);

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
