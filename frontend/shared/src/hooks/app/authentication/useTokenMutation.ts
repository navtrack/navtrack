import queryString from "query-string";
import { useMutation } from "react-query";
import { axiosInstance } from "../../../api/axiosInstance";
import { AuthenticationErrorType, getExpiryDate } from "./authentication";
import { useAuthentication } from "./useAuthentication";

type TokenRequest = {
  grant_type: string;
  username?: string;
  password?: string;
  scope?: string;
  client_id: string;
  refresh_token?: string;
};

export type TokenResponse = {
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

export function useTokenMutation() {
  const authentication = useAuthentication();

  const query = useMutation<TokenResponse, TokenError, TokenRequest>(
    "connect/token",
    async (data: TokenRequest) =>
      axiosInstance<TokenResponse>({
        url: `/connect/token`,
        method: "post",
        data: queryString.stringify(data),
        headers: {
          "Content-Type": "application/x-www-form-urlencoded"
        }
      }),
    {
      onMutate: async () => {
        await authentication.clearErrors();
      },
      onSuccess: async (data, variables) => {
        await authentication.set({
          token: {
            accessToken: data.access_token,
            refreshToken: data.refresh_token,
            expiryDate: getExpiryDate(data.expires_in),
            date: new Date().toISOString()
          }
        });
      },
      onError: async (_, data) => {
        const error =
          data.grant_type === "password"
            ? AuthenticationErrorType.Internal
            : data.grant_type === "refresh_token"
            ? AuthenticationErrorType.Other
            : AuthenticationErrorType.External;

        authentication.clear(error);
      }
    }
  );

  return query;
}
