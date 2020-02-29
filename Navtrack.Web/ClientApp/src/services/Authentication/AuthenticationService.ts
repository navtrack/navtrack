import { AppContextAccessor } from "services/AppContext/AppContextAccessor";

import { DefaultAuthenticationInfo, AuthenticationInfo } from "./AuthenticationInfo";
import { TokenResponse } from "services/Api/types/identity/TokenResponse";
import { IdentityApi } from "services/Api/IdentityApi";

export const AuthenticationService = {
  clearAuthenticationInfo: () => {
    const appContext = AppContextAccessor.getAppContext();

    AppContextAccessor.setAppContext({
      ...appContext,
      authenticationInfo: DefaultAuthenticationInfo
    });
  },

  login: async (email: string, password: string): Promise<boolean> => {
    const tokenResponse = await IdentityApi.login(email, password);

    if (tokenResponse.error) {
      return false;
    } else {
      const appContext = AppContextAccessor.getAppContext();

      AppContextAccessor.setAppContext({
        ...appContext,
        authenticationInfo: getNewAuthenticationInfo(email, tokenResponse)
      });

      return true;
    }
  }

  // checkAndRenewAccessToken: async (): Promise<boolean> => {
  //   if (AppContextAccessor.getAppContext().localStorageInfo.authenticationInfo.authenticated) {
  //     if (accessTokenIsExpired()) {
  //       const identityResponse: IdentityResponse = await IdentityApi.refreshAccessToken();

  //       if (renewFailed(identityResponse)) {
  //         IdentityService.clearAuthenticationInfo(true);

  //         return false;
  //       }

  //       const appContext = AppContextAccessor.getAppContext();
  //       const authenticationInfo = getNewAuthenticationInfo(
  //         identityResponse,
  //         appContext.localStorageInfo.authenticationInfo
  //       );

  //       AppContextAccessor.setAppContext({
  //         ...appContext,
  //         localStorageInfo: { ...appContext.localStorageInfo, authenticationInfo: authenticationInfo }
  //       });
  //     }

  //     return true;
  //   }

  //   return false;
  // }
};

const getNewAuthenticationInfo = (
  email: string,
  tokenResponse: TokenResponse
): AuthenticationInfo => {
  const newAuthenticationInfo: AuthenticationInfo = {
    email: email,
    authenticated: true,
    access_token: tokenResponse.access_token,
    refresh_token: tokenResponse.refresh_token,
    expiry_date: new Date(new Date().getTime() + tokenResponse.expires_in * 1000).toString()
  };

  return newAuthenticationInfo;
};
