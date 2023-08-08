import { useRecoilValue } from "recoil";
import { PublicClientApplication } from "@azure/msal-browser";
import { useMemo } from "react";
import { appConfigAtom } from "@navtrack/shared/state/appConfig";

export function useMicrosoftLogin() {
  const appConfig = useRecoilValue(appConfigAtom);

  const publicClientApplication = useMemo(
    () =>
      !!appConfig?.authentication?.microsoft?.clientId &&
      !!appConfig.authentication.microsoft.authority
        ? new PublicClientApplication({
            auth: {
              clientId: appConfig?.authentication.microsoft.clientId,
              authority: appConfig?.authentication.microsoft.authority
            },
            cache: {
              cacheLocation: "sessionStorage"
            }
          })
        : undefined,
    [appConfig?.authentication?.microsoft]
  );

  return { publicClientApplication };
}
