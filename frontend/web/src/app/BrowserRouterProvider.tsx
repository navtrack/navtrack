import { BrowserRouter } from "react-router-dom";
import { useRecoilValue } from "recoil";
import { isAuthenticatedAtom } from "@navtrack/shared/state/authentication";
import { ReactNode } from "react";

type BrowserRouterProviderProps = {
  privateRoutes: ReactNode;
  publicRoutes: ReactNode;
};

export function BrowserRouterProvider(props: BrowserRouterProviderProps) {
  const isAuthenticated = useRecoilValue(isAuthenticatedAtom);

  return (
    <BrowserRouter>
      {isAuthenticated ? props.privateRoutes : props.publicRoutes}
    </BrowserRouter>
  );
}
