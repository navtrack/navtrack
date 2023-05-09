import { faApple } from "@fortawesome/free-brands-svg-icons";
import { ExternalLoginButton } from "./ExternalLoginButton";
import { FormattedMessage } from "react-intl";
import { useRecoilValue } from "recoil";
import AppleSignin from "react-apple-signin-auth";
import { environmentSettingsSelector } from "@navtrack/shared/state/environment";

export function ExternalLoginButtonApple(props) {
  const settings = useRecoilValue(environmentSettingsSelector);

  return (
    <>
      {settings["AppleAuthentication.ClientId"] && (
        <AppleSignin
          authOptions={{
            clientId: settings["AppleAuthentication.ClientId"],
            scope: "email",
            redirectURI: settings["AppleAuthentication.RedirectUri"],
            usePopup: true
          }}
          onSuccess={(response) => props.login(response.authorization.id_token, "apple")} 
          onError={(error) => console.error(error)} 
          skipScript={false}
          render={(props) => (
            <ExternalLoginButton icon={faApple} onClick={props.onClick}>
              <FormattedMessage id="generic.apple" />
            </ExternalLoginButton>
          )}
        />
      )}
    </>
  );
}
