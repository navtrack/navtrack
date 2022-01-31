import { useMutation, UseMutationOptions } from "react-query";
import { axiosInstance } from "@navtrack/navtrack-shared";

type TokenRequest = {
  grant_type: string;
  username?: string;
  password?: string;
  scope?: string;
  client_id: string;
  refresh_token?: string;
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

export const useGetTokenMutation = (
  options: UseMutationOptions<TokenResponse, TokenError, TokenRequest>
) => {
  const query = useMutation(
    "connect/token",
    (data: TokenRequest) =>
      axiosInstance<TokenResponse>({
        url: `/connect/token`,
        method: "post",
        data: getURLSearchParams(data),
        headers: {
          "Content-Type": "application/x-www-form-urlencoded"
        }
      }),
    options
  );

  return query;
};

function getURLSearchParams(object: any) {
  const formData = new URLSearchParams();
  Object.keys(object).forEach((key) => formData.append(key, object[key]));
  return formData;
}
