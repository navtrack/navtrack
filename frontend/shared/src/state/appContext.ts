import { selector } from "recoil";
import { authenticationAtom } from "./authentication";
import { axiosConfigAtom } from "./axiosConfig";
import { appConfigAtom } from "./appConfig";

export const appContextSelector = selector({
  key: "Navtrack:AppContext",
  get: ({ get }) => {
    const appConfig = get(appConfigAtom);
    const axiosConfig = get(axiosConfigAtom);
    const authentication = get(authenticationAtom);

    return {
      initialized:
        appConfig !== undefined &&
        axiosConfig.baseUrlSet &&
        authentication.initialized &&
        (!authentication.isAuthenticated || axiosConfig.accessTokenSet),
      authentication: {
        accessToken: authentication.token?.accessToken,
        isAuthenticated: authentication.isAuthenticated
      }
    };
  }
});
