import { AccountApi } from "../apis/AccountApi";
import { AppContextAccessor } from "./appContext/AppContextAccessor";

export const AccountService = {
  getUserInfo: async () => {
    const appContext = AppContextAccessor.getAppContext();

    if (appContext.authenticationInfo.authenticated && !appContext.user) {
      const user = await AccountApi.get();

      AppContextAccessor.setAppContext({ ...AppContextAccessor.getAppContext(), user: user });
    }
  }
};
