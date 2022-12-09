import { faMicrosoft } from "@fortawesome/free-brands-svg-icons";
import { ExternalLoginButton } from "./ExternalLoginButton";
import { FormattedMessage } from "react-intl";
import { useMsal } from "@azure/msal-react";
import { useCallback } from "react";
import { useRecoilValue } from "recoil";
import { settingsSelector } from "@navtrack/navtrack-app-shared";
import { ICustomExternalLoginButton } from "./types";

export default function ExternalLoginButtonMicrosoft(
  props: ICustomExternalLoginButton
) {
  const settings = useRecoilValue(settingsSelector);
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
      {settings["MicrosoftAuthentication.ClientId"] && (
        <ExternalLoginButton icon={faMicrosoft} onClick={handleMicrosoftLogin}>
          <FormattedMessage id="generic.microsoft" />
        </ExternalLoginButton>
      )}
    </>
  );
}
