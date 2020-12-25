import { useMutation, useQuery } from "react-query";
import { formatUrl, rootUrl } from "../httpClient/HttpClientUtil";
import queryString from "query-string";

type Props<TRequest> = {
  url: string;
  method?: "GET" | "POST";
  enabled?: (request?: TRequest) => boolean;
};

const createQuery = <TRequest, TResponse>(props: Props<TRequest>) => (
  request: TRequest | undefined
) =>
  useQuery(
    props.url,
    async () => {
      const response = await fetch(formatUrl(rootUrl(props.url)), {
        method: "POST",
        headers: {
          "Content-Type": "application/x-www-form-urlencoded"
        },
        body: queryString.stringify(request as any)
      });

      const json = await response.json();

      return json as TResponse;
    },
    {
      enabled: props.enabled ? props.enabled(request) : true
    }
  );

type CreateMutationProps<TRequest> = {
  url: string;
  method?: "GET" | "POST";
};

export const createMutation = <TRequest, TResponse>(props: CreateMutationProps<TRequest>) => (
  request: TRequest | undefined
) => {
  const mutationFn = async (request: TRequest): Promise<TResponse> => {
    const response = await fetch(formatUrl(rootUrl(props.url)), {
      method: "POST",
      headers: {
        "Content-Type": "application/x-www-form-urlencoded"
      },
      body: queryString.stringify(request as any)
    });

    const json = await response.json();

    return json as TResponse;
  };

  return useMutation<TResponse, any, TRequest>(mutationFn);
};
