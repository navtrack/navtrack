import { UserModel } from "../apis/types/user/UserModel";
import { UserApi } from "../apis/UserApi";
import { AppContextAccessor } from "./appContext/AppContextAccessor";

export const UserService = {
  fetchCurrentUser: async (): Promise<void> => {
    const appContext = AppContextAccessor.getAppContext();

    if (appContext.authenticationInfo.authenticated && !appContext.user) {
      const user = await UserApi.get();

      AppContextAccessor.setAppContext({ ...AppContextAccessor.getAppContext(), user: user });
    }
  },

  getUser: async (): Promise<UserModel> => {
    const appContext = AppContextAccessor.getAppContext();

    if (appContext.user) {
      return appContext.user;
    }

    throw new Error("The user is not logged in.");
  }
};
