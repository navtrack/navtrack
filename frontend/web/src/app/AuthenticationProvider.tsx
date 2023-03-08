import { useAuthentication } from "@navtrack/shared/hooks/authentication/useAuthentication";
import { useAxiosAuthorization } from "@navtrack/shared/hooks/axios/useAxiosAuthorization";
import { ReactNode } from "react";
import { AUTHENTICATION } from "../constants";

export const AuthenticationProvider = (props: { children: ReactNode }) => {
  useAxiosAuthorization();
  useAuthentication({
    clientId: AUTHENTICATION.CLIENT_ID
  });

  return <>{props.children}</>;
};
