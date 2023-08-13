import queryString from "query-string";
import { useMutation } from "react-query";
import { axiosInstance } from "../../../api/axiosInstance";
import { AuthenticationErrorType } from "./authentication";
import { useAuthentication } from "./useAuthentication";
import { add } from "date-fns";
import { useSetRecoilState } from "recoil";
import { isLoggingInAtom } from "../../../state/authentication";

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

function getExpiryDate(expiresIn: number) {
  const date = add(new Date(), {
    seconds: expiresIn
  }).toISOString();

  return date;
}

export function useTokenMutation() {
  const authentication = useAuthentication();
  const setIsLoggingIn = useSetRecoilState(isLoggingInAtom);

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
      onMutate: () => {
        setIsLoggingIn(true);
        authentication.clearErrors();
      },
      onSuccess: async (data) =>
        authentication.set({
          token: {
            accessToken: data.access_token,
            refreshToken: data.refresh_token,
            expiryDate: getExpiryDate(data.expires_in),
            date: new Date().toISOString()
          }
        }),
      onError: (_, data) => {
        const error =
          data.grant_type === "password"
            ? AuthenticationErrorType.Internal
            : data.grant_type === "refresh_token"
            ? AuthenticationErrorType.Other
            : AuthenticationErrorType.External;

        return authentication.clear(error);
      },
      onSettled: () => {
        setIsLoggingIn(false);
      }
    }
  );

  return query;
}
