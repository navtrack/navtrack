import { faGoogle } from "@fortawesome/free-brands-svg-icons";
import { ExternalLoginButton } from "./ExternalLoginButton";
import { FormattedMessage } from "react-intl";
import { useRecoilValue } from "recoil";
import { CustomExternalLoginButtonProps } from "./types";
import { useGoogleLogin } from "@react-oauth/google";
import { appConfigAtom } from "@navtrack/shared/state/appConfig";
import { log } from "@navtrack/shared/utils/log";

export function ExternalLoginButtonGoogle(
  props: CustomExternalLoginButtonProps
) {
  const appConfig = useRecoilValue(appConfigAtom);

  const login = useGoogleLogin({
    flow: "auth-code",
    onSuccess: (tokenResponse) => props.login(tokenResponse.code, "google"),
    onError: (error) => log(error)
  });

  return (
    <>
      {appConfig?.authentication?.google?.clientId && (
        <ExternalLoginButton icon={faGoogle} onClick={login}>
          <FormattedMessage id="generic.google" />
        </ExternalLoginButton>
      )}
    </>
  );
}
