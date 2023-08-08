import { FormattedMessage } from "react-intl";
import { useMemo } from "react";
import { MsalProvider } from "@azure/msal-react";
import { ExternalLoginButtonApple } from "./ExternalLoginButtonApple";
import { ExternalLoginButtonMicrosoft } from "./ExternalLoginButtonMicrosoft";
import { ExternalLoginButtonGoogle } from "./ExternalLoginButtonGoogle";
import { useRecoilValue } from "recoil";
import { useMicrosoftLogin } from "./useMicrosoftLogin";
import { GoogleOAuthProvider } from "@react-oauth/google";
import { appConfigAtom } from "@navtrack/shared/state/appConfig";

type ExternalLoginProps = {
  login: (code: string, grantType: "apple" | "microsoft" | "google") => void;
};

export function ExternalLogin(props: ExternalLoginProps) {
  const { publicClientApplication } = useMicrosoftLogin();
  const appConfig = useRecoilValue(appConfigAtom);

  const hasExternalLogins = useMemo(
    () =>
      !!appConfig?.authentication?.google?.clientId ||
      !!appConfig?.authentication?.microsoft?.clientId ||
      !!appConfig?.authentication?.apple?.clientId,
    [appConfig?.authentication]
  );

  const buttons = useMemo(
    () => (
      <>
        <div className="my-4 flex items-center justify-center text-xs">
          <div className="flex-grow bg-gray-300 " style={{ height: "1px" }} />
          <div className="flex justify-center px-2  font-light text-gray-400">
            <FormattedMessage id="login.external.sign-in" />
          </div>
          <div className="flex-grow bg-gray-300" style={{ height: "1px" }} />
        </div>
        <div className="grid grid-cols-3 gap-2">
          <ExternalLoginButtonApple login={props.login} />
          <ExternalLoginButtonMicrosoft login={props.login} />
          {!!appConfig?.authentication?.google?.clientId && (
            <GoogleOAuthProvider
              clientId={appConfig.authentication.google.clientId}>
              <ExternalLoginButtonGoogle login={props.login} />
            </GoogleOAuthProvider>
          )}
        </div>
      </>
    ),
    [appConfig?.authentication, props.login]
  );

  return hasExternalLogins ? (
    <>
      {publicClientApplication ? (
        <MsalProvider instance={publicClientApplication}>
          {buttons}
        </MsalProvider>
      ) : (
        <>{buttons}</>
      )}
    </>
  ) : null;
}
