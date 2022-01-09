import { selector } from "recoil";
import { authenticationAtom } from "./app.authentication";
import { axiosAtom } from "./app.axios";
import { configAtom } from "./app.config";
import { localStorageAtom } from "./app.localStorage";

export const appContextSelector = selector({
  key: "App:Context",
  get: ({ get }) => {
    const config = get(configAtom);
    const localStorage = get(localStorageAtom);
    const authentication = get(authenticationAtom);
    const axios = get(axiosAtom);

    return {
      initialized:
        config.initialized &&
        localStorage.initialized &&
        authentication.initialized,
      config: config,
      axios: axios,
      authentication: authentication,
      localStorage: localStorage
    };
  }
});
