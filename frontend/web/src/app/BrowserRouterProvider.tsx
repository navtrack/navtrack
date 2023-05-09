import { appContextSelector } from "@navtrack/shared/state/appContext";
import { BrowserRouter } from "react-router-dom";
import { useRecoilValue } from "recoil";
import { RoutesAuthenticated } from "./RoutesAuthenticated";
import { RoutesUnauthenticated } from "./RoutesUnauthenticated";

export const BrowserRouterProvider = () => {
  const appContext = useRecoilValue(appContextSelector);

  return (
    <BrowserRouter>
      {appContext.initialized ? (
        appContext.authentication.isAuthenticated ? (
          <RoutesAuthenticated />
        ) : (
          <RoutesUnauthenticated />
        )
      ) : (
        <></>
      )}
    </BrowserRouter>
  );
};
