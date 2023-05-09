import { ReactNode } from "react";
import { useAuthentication } from "../hooks/app/authentication/useAuthentication";
import { useAxiosAuthorization } from "../hooks/app/axios/useAxiosAuthorization";

type AuthenticationProviderProps = {
  children: ReactNode;
  clientId: string;
};

export function AuthenticationProvider(props: AuthenticationProviderProps) {
  useAuthentication({ clientId: props.clientId });
  useAxiosAuthorization();

  return <>{props.children}</>;
}
