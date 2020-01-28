import { AppContextAccessor } from "./AppContext/AppContextAccessor";
import { AccountApi } from "./Api/AccountApi";

export const AccountService = {
  getUserInfo: async () => {
    const appContext = AppContextAccessor.getAppContext();

    if (appContext.authenticated && !appContext.user) {
      const user = await AccountApi.get();

      AppContextAccessor.setAppContext({ ...AppContextAccessor.getAppContext(), user: user });
    }
  }
};