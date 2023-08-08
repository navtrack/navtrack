import { faMicrosoft } from "@fortawesome/free-brands-svg-icons";
import { ExternalLoginButton } from "./ExternalLoginButton";
import { FormattedMessage } from "react-intl";
import { useMsal } from "@azure/msal-react";
import { useCallback } from "react";
import { useRecoilValue } from "recoil";
import { CustomExternalLoginButtonProps } from "./types";
import { appConfigAtom } from "@navtrack/shared/state/appConfig";

export function ExternalLoginButtonMicrosoft(
  props: CustomExternalLoginButtonProps
) {
  const appConfig = useRecoilValue(appConfigAtom);
  const { instance } = useMsal();

  const handleMicrosoftLogin = useCallback(() => {
    instance
      .loginPopup({
        scopes: ["email"]
      })
      .then((result) => props.login(result.idToken, "microsoft"))
      .catch((e) => console.error(e));
  }, [instance, props]);

  return (
    <>
      {appConfig?.authentication?.microsoft?.clientId && (
        <ExternalLoginButton icon={faMicrosoft} onClick={handleMicrosoftLogin}>
          <FormattedMessage id="generic.microsoft" />
        </ExternalLoginButton>
      )}
    </>
  );
}
