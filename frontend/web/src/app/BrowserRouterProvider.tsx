import { BrowserRouter } from "react-router-dom";
import { useRecoilValue } from "recoil";
import { RoutesAuthenticated } from "./RoutesAuthenticated";
import { RoutesUnauthenticated } from "./RoutesUnauthenticated";
import { isAuthenticatedAtom } from "@navtrack/shared/state/authentication";

export function BrowserRouterProvider() {
  const isAuthenticated = useRecoilValue(isAuthenticatedAtom);

  return (
    <BrowserRouter>
      {isAuthenticated ? <RoutesAuthenticated /> : <RoutesUnauthenticated />}
    </BrowserRouter>
  );
}
