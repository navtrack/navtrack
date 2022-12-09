import { useAuthentication } from "@navtrack/ui-shared/hooks/authentication/useAuthentication";
import { useAxiosAuthorization } from "@navtrack/ui-shared/hooks/axios/useAxiosAuthorization";
import { ReactNode } from "react";
import { AUTHENTICATION } from "../constants";

export const AuthenticationProvider = (props: { children: ReactNode }) => {
  useAxiosAuthorization();
  useAuthentication({
    clientId: AUTHENTICATION.CLIENT_ID
  });

  return <>{props.children}</>;
};
