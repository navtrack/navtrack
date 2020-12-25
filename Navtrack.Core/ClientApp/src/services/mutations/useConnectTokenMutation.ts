import { createMutation } from "../queries/createMutation";

type TokenRequest = {
  grant_type: string;
  username: string;
  password: string;
  scope: string;
  client_id: string;
};

type TokenResponse = {
  access_token: string;
  refresh_token: string;
  expires_in: number;
  token_type: string;
  scope: string;
  error: string;
};

type TokenError = {
  error: string;
  error_description: string;
};

export const useConnectTokenMutation = createMutation<TokenRequest, TokenResponse, TokenError>({
  url: "connect/token",
  contentType: "application/x-www-form-urlencoded"
});
