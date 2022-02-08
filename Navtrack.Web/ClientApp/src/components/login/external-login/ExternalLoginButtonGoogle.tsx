import GoogleLogin, {
  GoogleLoginResponse,
  GoogleLoginResponseOffline
} from "react-google-login";
import { faGoogle } from "@fortawesome/free-brands-svg-icons";
import { ExternalLoginButton } from "./ExternalLoginButton";
import { FormattedMessage } from "react-intl";
import { useRecoilValue } from "recoil";
import { settingsSelector } from "@navtrack/navtrack-app-shared";
import { ICustomExternalLoginButton } from "./types";

export default function ExternalLoginButtonGoogle(
  props: ICustomExternalLoginButton
) {
  const settings = useRecoilValue(settingsSelector);

  const handleGoogleSuccess = (
    response: GoogleLoginResponse | GoogleLoginResponseOffline
  ) => {
    const googleLoginResponse = response as GoogleLoginResponse;

    if (googleLoginResponse.tokenObj !== undefined) {
      props.login(googleLoginResponse.tokenObj.id_token, "google");
    }
  };

  return (
    <>
      {settings["GoogleAuthentication.ClientId"] && (
        <GoogleLogin
          clientId={settings["GoogleAuthentication.ClientId"]}
          onSuccess={handleGoogleSuccess}
          onFailure={(error) => console.log(error)}
          render={(renderProps) => (
            <ExternalLoginButton icon={faGoogle} onClick={renderProps.onClick}>
              <FormattedMessage id="generic.google" />
            </ExternalLoginButton>
          )}
        />
      )}
    </>
  );
}
