import { TokenResponse } from "./types/identity/TokenResponse";
import { HttpClient } from "services/HttpClient/HttpClient";
import { UserInfo } from "./types/identity/UserInfo";
import { identityUrl, formatUrl } from "services/HttpClient/HttpClientUtil";

export const IdentityApi = {
  login: async (username: string, password: string): Promise<TokenResponse> => {
    let body = {
      grant_type: "password",
      username: username,
      password: password,
      scope: "IdentityServerApi openid",
      client_id: "navtrack.web"
    };

    return HttpClient.post<TokenResponse>(
      formatUrl(identityUrl("connect/token")),
      body,
      "application/x-www-form-urlencoded"
    );
  },

  getUserInfo: async (): Promise<UserInfo> => {
    return HttpClient.get<UserInfo>(identityUrl("connect/userinfo"));
  }
};
