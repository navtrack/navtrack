import { AppContextAccessor } from "services/appContext/AppContextAccessor";
import { UserRole } from "services/api/types/user/UserRole";

export const AuthorizationService = {
  isAuthorized: (userRole?: UserRole) => {
    var appContext = AppContextAccessor.getAppContext();

    return (
      appContext.authenticationInfo.authenticated &&
      (userRole === undefined ||
        (appContext.user !== undefined && appContext.user.role === userRole))
    );
  }
};
