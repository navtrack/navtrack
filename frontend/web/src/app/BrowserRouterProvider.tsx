import { BrowserRouter } from "react-router-dom";
import { ReactNode } from "react";
import { useAuthentication } from "@navtrack/shared/hooks/app/authentication/useAuthentication";
import { CurrentContextProvider } from "@navtrack/shared/hooks/current/CurrentContextProvider";

type BrowserRouterProviderProps = {
  privateRoutes: ReactNode;
  publicRoutes: ReactNode;
};

export function BrowserRouterProvider(props: BrowserRouterProviderProps) {
  const authentication = useAuthentication();

  return (
    <BrowserRouter>
      <CurrentContextProvider>
        {authentication.isAuthenticated
          ? props.privateRoutes
          : props.publicRoutes}
      </CurrentContextProvider>
    </BrowserRouter>
  );
}
