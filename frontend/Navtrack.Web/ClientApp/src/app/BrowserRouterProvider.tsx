import { BrowserRouter } from "react-router-dom";
import { useRecoilValue } from "recoil";
import { appContextSelector } from "@navtrack/navtrack-app-shared";
import RoutesAuthenticated from "./RoutesAuthenticated";
import RoutesUnauthenticated from "./RoutesUnauthenticated";

export default function BrowserRouterProvider() {
  const appContext = useRecoilValue(appContextSelector);

  return (
    <BrowserRouter>
      {appContext.initialized && (
        <>
          {appContext.authentication.isAuthenticated ? (
            <>{appContext.axios?.initialized && <RoutesAuthenticated />}</>
          ) : (
            <RoutesUnauthenticated />
          )}
        </>
      )}
    </BrowserRouter>
  );
}
