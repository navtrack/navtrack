import queryString from "query-string";
import { axiosInstance } from "../../../axios/axiosInstance";
import { useMutation, UseMutationOptions } from "@tanstack/react-query";
import { AxiosError } from "axios";

export type TokenRequest = {
  grant_type: string;
  code?: string;
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
  code?: string;
  token?: string;
};

type UseTokenMutationProps = {
  options?: Omit<
    UseMutationOptions<TokenResponse, AxiosError<TokenError>, TokenRequest>,
    "mutationFn"
  >;
};

export function useTokenMutation(props: UseTokenMutationProps) {
  const mutation = useMutation<
    TokenResponse,
    AxiosError<TokenError>,
    TokenRequest
  >(
    async (data: TokenRequest) =>
      axiosInstance<TokenResponse>({
        url: `/connect/token`,
        method: "post",
        data: queryString.stringify(data),
        headers: {
          "Content-Type": "application/x-www-form-urlencoded"
        }
      }),
    props.options
  );

  return mutation;
}
