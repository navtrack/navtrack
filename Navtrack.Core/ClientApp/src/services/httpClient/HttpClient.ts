import queryString from "query-string";
import { ResponseModel } from "../../apis/types/ResponseModel";
import { AppContextAccessor } from "../appContext/AppContextAccessor";
import { AuthenticationService } from "../authentication/AuthenticationService";
import { ValidationResult } from "../validation/ValidationResult";
import { ApiError } from "./AppError";

export const HttpClient = {
  get: async <T>(url: string, body?: any) => {
    await AuthenticationService.checkAndRenewAccessToken();

    return fetch(url, {
      method: "GET",
      credentials: "include",
      headers: getHeaders()
    }).then((response) => handleResponse<T>(response));
  },

  post: async <T>(
    url: string,
    body: any,
    contentType?: "application/x-www-form-urlencoded" | undefined,
    skipAccessTokenCheck?: boolean
  ): Promise<T> => {
    if (!skipAccessTokenCheck) {
      await AuthenticationService.checkAndRenewAccessToken();
    }

    return fetch(url, {
      method: "POST",
      credentials: "include",
      headers: getHeaders(contentType),
      body: getBody(body, contentType)
    }).then((response) => handleResponse<T>(response));
  },

  delete: async (url: string): Promise<ResponseModel> => {
    await AuthenticationService.checkAndRenewAccessToken();

    return fetch(url, {
      method: "DELETE",
      credentials: "include",
      headers: getHeaders()
    }).then((x) => handleResponse<ResponseModel>(x));
  },

  put: async (url: string, body: any): Promise<ResponseModel> => {
    await AuthenticationService.checkAndRenewAccessToken();

    return fetch(url, {
      method: "PUT",
      credentials: "include",
      headers: getHeaders(),
      body: getBody(body)
    }).then((x) => handleResponse<ResponseModel>(x));
  },

  patch: async <T>(
    url: string,
    body: any,
    contentType?: "application/x-www-form-urlencoded" | undefined,
    skipAccessTokenCheck?: boolean
  ): Promise<T> => {
    if (!skipAccessTokenCheck) {
      await AuthenticationService.checkAndRenewAccessToken();
    }

    return fetch(url, {
      method: "PATCH",
      credentials: "include",
      headers: getHeaders(contentType),
      body: getBody(body, contentType)
    }).then((response) => handleResponse<T>(response));
  }
};

function getBody(body: any, contentType?: "application/x-www-form-urlencoded" | undefined) {
  if (contentType === "application/x-www-form-urlencoded") {
    return queryString.stringify(body);
  }

  return JSON.stringify(body);
}

function getHeaders(contentType?: string | undefined): Record<string, string> {
  let headers: Record<string, string> = {};
  let appContext = AppContextAccessor.getAppContext();

  if (appContext.authenticationInfo.authenticated && appContext.authenticationInfo.access_token) {
    headers["Authorization"] = `Bearer ${appContext.authenticationInfo.access_token}`;
  }
  headers["Content-Type"] = contentType ? contentType : "application/json";

  return headers;
}

// TODO rewrite this
async function handleResponse<T>(response: Response): Promise<T> {
  if (response.ok) {
    try {
      const json = await response.json();

      return json as T;
    } catch (error) {
      const responseModel = {
        status: response.status
      };

      return (responseModel as unknown) as T;
    }
  }

  let json: { errors: Partial<Record<keyof T, string[]>> } = { errors: {} };

  try {
    json = await response.json();
    // eslint-disable-next-line no-empty
  } catch (error) {}

  const validationResult = new ValidationResult<T>();

  for (const key in json.errors) {
    const element = json.errors[key];

    element?.forEach((error) => {
      validationResult.AddError(key, error);
    });
  }

  let error = new ApiError<T>(validationResult);

  throw error;
}
