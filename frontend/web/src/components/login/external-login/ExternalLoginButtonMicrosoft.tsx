import { faMicrosoft } from "@fortawesome/free-brands-svg-icons";
import { ExternalLoginButton } from "./ExternalLoginButton";
import { FormattedMessage } from "react-intl";
import { useMsal } from "@azure/msal-react";
import { useCallback } from "react";
import { useRecoilValue } from "recoil";
import { ICustomExternalLoginButton } from "./types";
import { settingsSelector } from "@navtrack/ui-shared/state/app.settings";

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
