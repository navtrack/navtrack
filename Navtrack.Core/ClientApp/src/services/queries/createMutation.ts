import { useMutation } from "react-query";
import { formatUrl, rootUrl } from "../httpClient/HttpClientUtil";
import queryString from "query-string";
import { useIntl } from "react-intl";
import { ApiError } from "../ApiError";

type ContentType = "application/x-www-form-urlencoded";

type CreateMutationProps = {
  url: string;
  method?: "GET" | "POST";
  contentType?: ContentType;
};

const getHeaders = (contentType?: ContentType): Record<string, string> => {
  let headers: Record<string, string> = {};

  headers["Content-Type"] = contentType ? contentType : "application/json";

  return headers;
};

const getBody = (body: any, contentType?: ContentType) => {
  if (contentType === "application/x-www-form-urlencoded") {
    return queryString.stringify(body);
  }

  return JSON.stringify(body);
};

export const createMutation = <TRequest, TResponse = any | undefined, TError = ApiError>(
  props: CreateMutationProps
) => () => {
  const intl = useIntl();

  const mutationFn = async (request?: TRequest): Promise<TResponse | undefined> => {
    const response = await fetch(formatUrl(rootUrl(props.url)), {
      method: "POST",
      headers: getHeaders(props.contentType),
      body: getBody(request, props.contentType)
    });

    if (response.ok) {
      try {
        const json = await response.json();

        return json as TResponse;
      } catch {
        return undefined;
      }
    }

    try {
      const json = await response.json();

      return Promise.reject(json as TError);
    } catch {
      const error: ApiError = { message: intl.formatMessage({ id: "generic.error" }) };

      return Promise.reject(error);
    }
  };

  return useMutation<TResponse | undefined, TError, TRequest>(mutationFn, {});
};
