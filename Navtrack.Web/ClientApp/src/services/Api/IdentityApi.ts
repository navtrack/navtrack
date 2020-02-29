import queryString from "query-string";
import { TokenResponse } from "./types/identity/TokenResponse";
import { HttpClient } from "services/HttpClient/HttpClient";
import { UserInfo } from "./types/identity/UserInfo";
import { identityUrl } from "services/HttpClient/HttpClientUtil";

export const IdentityApi = {
  login: async (username: string, password: string): Promise<TokenResponse> => {
    let body = queryString.stringify({
      grant_type: "password",
      username: username,
      password: password,
      scope: "IdentityServerApi openid",
      client_id: "navtrack.web"
    });

    return HttpClient.post2<TokenResponse>("connect/token", body);
  },

  getUserInfo: async (): Promise<UserInfo> => {
    return HttpClient.get<UserInfo>(identityUrl("connect/userinfo"));
  }
};
