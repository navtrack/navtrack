import { BrowserRouter } from "react-router-dom";
import { useRecoilValue } from "recoil";
import { appContextSelector } from "../state/app.context";
import RoutesAuthenticated from "./RoutesAuthenticated";
import RoutesUnauthenticated from "./RoutesUnauthenticated";

export default function BrowserRouterProvider() {
  const appContext = useRecoilValue(appContextSelector);

  console.log(appContext);

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
