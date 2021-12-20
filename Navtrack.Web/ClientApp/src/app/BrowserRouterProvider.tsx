import { BrowserRouter } from "react-router-dom";
import { useRecoilValue } from "recoil";
import { axiosAuthAtom } from "../api/axiosAuthAtom";
import useAppContext from "../hooks/app/useAppContext";
import RoutesAuthenticated from "./RoutesAuthenticated";
import RoutesUnauthenticated from "./RoutesUnauthenticated";

export default function BrowserRouterProvider() {
  const { appContext } = useAppContext();
  const axiosAuthState = useRecoilValue(axiosAuthAtom);

  return (
    <BrowserRouter>
      {appContext.initialized && (
        <>
          {appContext.isAuthenticated && axiosAuthState?.interceptorInit ? (
            <RoutesAuthenticated />
          ) : (
            <RoutesUnauthenticated />
          )}
        </>
      )}
    </BrowserRouter>
  );
}
