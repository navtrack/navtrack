import { faApple } from "@fortawesome/free-brands-svg-icons";
import { ExternalLoginButton } from "./ExternalLoginButton";
import { FormattedMessage } from "react-intl";
import { useRecoilValue } from "recoil";
import AppleSignin from "react-apple-signin-auth";
import { appConfigAtom } from "@navtrack/shared/state/appConfig";

export function ExternalLoginButtonApple(props) {
  const appConfig = useRecoilValue(appConfigAtom);

  return (
    <>
      {appConfig?.authentication?.apple?.clientId && (
        <AppleSignin
          authOptions={{
            clientId: appConfig.authentication.apple.clientId,
            scope: "email",
            redirectURI: appConfig?.authentication.apple.redirectUri,
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
