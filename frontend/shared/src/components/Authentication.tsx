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

  const [refreshed, setRefreshed] = useState(false);

  useEffect(() => {
    if (isAuthenticated) {
      if (!refreshed && !accessToken.isLoading) {
        log("AUTHENTICATION - !refreshed");
        accessToken.getAccessToken().then((x) => {
          setRefreshed(true);
          log("AUTHENTICATION - refreshed");
        });
      }
    } else {
      log("AUTHENTICATION - !authenticated");
      setRefreshed(true);
    }
  }, [accessToken, isAuthenticated, refreshed]);

  return <>{refreshed && props.children}</>;
}
