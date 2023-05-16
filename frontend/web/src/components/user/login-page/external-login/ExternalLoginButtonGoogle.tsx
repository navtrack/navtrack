import { faGoogle } from "@fortawesome/free-brands-svg-icons";
import { ExternalLoginButton } from "./ExternalLoginButton";
import { FormattedMessage } from "react-intl";
import { useRecoilValue } from "recoil";
import { CustomExternalLoginButtonProps } from "./types";
import { environmentSettingsSelector } from "@navtrack/shared/state/environment";
import { useGoogleLogin } from "@react-oauth/google";

export function ExternalLoginButtonGoogle(
  props: CustomExternalLoginButtonProps
) {
  const settings = useRecoilValue(environmentSettingsSelector);

  const login = useGoogleLogin({
    flow: "auth-code",
    onSuccess: (tokenResponse) => props.login(tokenResponse.code, "google"),
    onError: (error) => console.log(error)
  });

  return (
    <>
      {settings["GoogleAuthentication.ClientId"] && (
        <ExternalLoginButton icon={faGoogle} onClick={login}>
          <FormattedMessage id="generic.google" />
        </ExternalLoginButton>
      )}
    </>
  );
}
