import { BrowserRouter } from "react-router-dom";
import { ReactNode } from "react";
import { useAuthentication } from "@navtrack/shared/hooks/app/authentication/useAuthentication";

type BrowserRouterProviderProps = {
  privateRoutes: ReactNode;
  publicRoutes: ReactNode;
};

export function BrowserRouterProvider(props: BrowserRouterProviderProps) {
  const authentication = useAuthentication();

  return (
    <BrowserRouter>
      {authentication.isAuthenticated
        ? props.privateRoutes
        : props.publicRoutes}
    </BrowserRouter>
  );
}
