import { FormattedMessage } from "react-intl";
import { useMemo } from "react";
import { MsalProvider } from "@azure/msal-react";
import ExternalLoginButtonApple from "./ExternalLoginButtonApple";
import ExternalLoginButtonMicrosoft from "./ExternalLoginButtonMicrosoft";
import ExternalLoginButtonGoogle from "./ExternalLoginButtonGoogle";
import { useRecoilValue } from "recoil";
import useMicrosoftLogin from "./useMicrosoftLogin";
import { settingsSelector } from "../../../state/app.settings";

interface IExternalLogin {
  login: (code: string, grantType: "apple" | "microsoft" | "google") => void;
}

export default function ExternalLogin(props: IExternalLogin) {
  const { publicClientApplication } = useMicrosoftLogin();
  const settings = useRecoilValue(settingsSelector);

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
        <div className="flex justify-center items-center text-xs my-4">
          <div className="bg-gray-300 flex-grow " style={{ height: "1px" }} />
          <div className="flex justify-center px-2  text-gray-400 font-light">
            <FormattedMessage id="login.external.sign-in" />
          </div>
          <div className="bg-gray-300 flex-grow" style={{ height: "1px" }} />
        </div>
        <div className="flex space-x-2">
          <ExternalLoginButtonApple login={props.login} />
          <ExternalLoginButtonMicrosoft login={props.login} />
          <ExternalLoginButtonGoogle login={props.login} />
        </div>
      </>
    ),
    [props.login]
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
