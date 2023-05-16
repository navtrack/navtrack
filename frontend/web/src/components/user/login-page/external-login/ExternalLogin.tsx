import { FormattedMessage } from "react-intl";
import { useMemo } from "react";
import { MsalProvider } from "@azure/msal-react";
import { ExternalLoginButtonApple } from "./ExternalLoginButtonApple";
import { ExternalLoginButtonMicrosoft } from "./ExternalLoginButtonMicrosoft";
import { ExternalLoginButtonGoogle } from "./ExternalLoginButtonGoogle";
import { useRecoilValue } from "recoil";
import { useMicrosoftLogin } from "./useMicrosoftLogin";
import { environmentSettingsSelector } from "@navtrack/shared/state/environment";
import { GoogleOAuthProvider } from "@react-oauth/google";

interface IExternalLogin {
  login: (code: string, grantType: "apple" | "microsoft" | "google") => void;
}

export function ExternalLogin(props: IExternalLogin) {
  const { publicClientApplication } = useMicrosoftLogin();
  const settings = useRecoilValue(environmentSettingsSelector);

  const hasExternalLogins = useMemo(
    () =>
      !!settings["GoogleAuthentication.ClientId"] ||
      !!settings["MicrosoftAuthentication.ClientId"] ||
      !!settings["AppleAuthentication.ClientId"],
    [settings]
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
          {!!settings["GoogleAuthentication.ClientId"] && (
            <GoogleOAuthProvider
              clientId={settings["GoogleAuthentication.ClientId"]}>
              <ExternalLoginButtonGoogle login={props.login} />
            </GoogleOAuthProvider>
          )}
        </div>
      </>
    ),
    [props.login, settings]
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
