import { ReactNode, useEffect, useState } from "react";
import { useAccessToken } from "../hooks/app/authentication/useAccessToken";
import { useRecoilValue } from "recoil";
import { isAuthenticatedAtom } from "../state/authentication";
import { log } from "../utils/log";

type AuthenticationProps = {
  children: ReactNode;
};

export function Authentication(props: AuthenticationProps) {
  const accessToken = useAccessToken();
  const isAuthenticated = useRecoilValue(isAuthenticatedAtom);
  const [checked, setChecked] = useState(false);

  useEffect(() => {
    if (isAuthenticated) {
      if (!checked && !accessToken.isLoading) {
        log("AUTHENTICATION - !checked");
        accessToken.getAccessToken().then((x) => {
          setChecked(true);
          log("AUTHENTICATION - checked");
        });
      }
    } else {
      log("AUTHENTICATION - !authenticated");
      setChecked(true);
    }
  }, [accessToken, isAuthenticated, checked]);

  return <>{checked && props.children}</>;
}
