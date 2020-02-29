import { AppContextAccessor } from "./AppContext/AppContextAccessor";
import { AccountApi } from "./Api/AccountApi";
import { IdentityApi } from "./Api/IdentityApi";

export const AccountService = {
  getUserInfo: async () => {
    const appContext = AppContextAccessor.getAppContext();

    if (appContext.authenticationInfo.authenticated && !appContext.user) {
      const user = await AccountApi.get();
      let userInfo = await IdentityApi.getUserInfo();

      AppContextAccessor.setAppContext({ ...AppContextAccessor.getAppContext(), user: user });
    }
  }
};
