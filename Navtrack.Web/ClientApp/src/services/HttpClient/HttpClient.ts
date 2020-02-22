import { HttpClientUtil } from "./HttpClientUtil";
import { ResponseModel } from "../Api/Model/ResponseModel";
import { ValidationResult } from "components/Common/ValidatonResult";
import { AppError } from "./AppError";
import queryString from "query-string";

export const HttpClient = {
    get: <T>(url: string, requestObject?: any) =>
        fetch(formatUrl(HttpClientUtil.apiUrl(url), requestObject), {
            method: "GET",
            credentials: "include"
        }).then(response => handleResponse<T>(response)),

    post: (url: string, bodyObject: any): Promise<ResponseModel> =>
        fetch(HttpClientUtil.apiUrl(url), {
            method: "POST",
            credentials: "include",
            headers: {
                "Content-Type": "application/json-patch+json"
            },
            body: JSON.stringify(bodyObject)
        }).then(x => handleResponse<ResponseModel>(x)),

    delete: (url: string): Promise<ResponseModel> =>
        fetch(HttpClientUtil.apiUrl(url), {
            method: "DELETE",
            credentials: "include",
            headers: {
                "Content-Type": "application/json-patch+json"
            }
        }).then(x => handleResponse<ResponseModel>(x)),

    put: (url: string, bodyObject: any): Promise<ResponseModel> =>
        fetch(HttpClientUtil.apiUrl(url), {
            method: "PUT",
            credentials: "include",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(bodyObject)
        }).then(x => handleResponse<ResponseModel>(x))
};

function formatUrl(url: string, bodyObject?: any): string {
    return queryString.stringifyUrl({ url: url, query: bodyObject });
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

    let json: { errors?: ValidationResult } = {};

    try {
        json = await response.json();
        // eslint-disable-next-line no-empty
    } catch (error) {}

    let error: AppError = { status: response.status, validationResult: json.errors ? json.errors : {}, message: "" };

    throw error;
}
