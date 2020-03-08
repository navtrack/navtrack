import { AppContextAccessor } from "services/AppContext/AppContextAccessor";
import { UserRole } from "services/Api/Model/UserRole";

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
