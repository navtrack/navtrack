import { TokenResponse } from "./types/identity/TokenResponse";
import { HttpClient } from "framework/httpClient/HttpClient";
import { UserInfo } from "./types/identity/UserInfo";
import { formatUrl, rootUrl } from "framework/httpClient/HttpClientUtil";

export const IdentityApi = {
  login: async (username: string, password: string): Promise<TokenResponse> => {
    let body = {
      grant_type: "password",
      username: username,
      password: password,
      scope: "offline_access IdentityServerApi openid",
      client_id: "navtrack.web"
    };

    return HttpClient.post<TokenResponse>(
      formatUrl(rootUrl("connect/token")),
      body,
      "application/x-www-form-urlencoded",
      true
    );
  },

  getUserInfo: async (): Promise<UserInfo> => {
    return HttpClient.get<UserInfo>(rootUrl("connect/userinfo"));
  },

  refreshToken: (refreshToken: string): Promise<TokenResponse> => {
    let body = {
      grant_type: "refresh_token",
      refresh_token: refreshToken,
      scope: "IdentityServerApi",
      client_id: "navtrack.web"
    };

    return HttpClient.post<TokenResponse>(
      formatUrl(rootUrl("connect/token")),
      body,
      "application/x-www-form-urlencoded",
      true
    );
  }
};
