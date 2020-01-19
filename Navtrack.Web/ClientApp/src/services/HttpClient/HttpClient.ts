import { HttpClientUtil } from "./HttpClientUtil";
import { ResponseModel } from "../Api/Model/ResponseModel";

export const HttpClient = {
    get: <T>(url: string) =>
        fetch(HttpClientUtil.apiUrl(url), {
            method: "GET",
            credentials: "include"
        }).then(response => handleResponse<T>(response)),

    post: (url: string, bodyObject: any): Promise<ResponseModel> =>
        fetch(HttpClientUtil.apiUrl(url), {
            method: "POST",
            credentials: "include",
            headers: {
                "Content-Type": "application/json-patch+json",
            },
            body: JSON.stringify(bodyObject)
        }).then(x => handleResponse<ResponseModel>(x)),

    delete: (url: string): Promise<ResponseModel> =>
        fetch(HttpClientUtil.apiUrl(url), {
            method: "DELETE",
            credentials: "include",
            headers: {
                "Content-Type": "application/json-patch+json",
            }
        }).then(x => handleResponse<ResponseModel>(x)),

    put: (url: string, bodyObject: any): Promise<ResponseModel> =>
        fetch(HttpClientUtil.apiUrl(url), {
            method: "PUT",
            credentials: "include",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(bodyObject)
        }).then(x => handleResponse<ResponseModel>(x))
};

export type Errors = Record<string, string[]>;

async function handleResponse<T>(response: Response): Promise<T> {
    if (response.ok) {
        try {
            const json = await response.json();

            return json as T;
        } catch (error) {
            const responseModel = {
                status: response.status
            };

            return responseModel as unknown as T;
        }
    } else {
        let json: { errors?: Errors } = {};

        try {
            json = await response.json();
        } catch (error) {
        }

        if (json.errors) {
            throw new ApiError(json.errors);
        }
    }

    throw new ApiError({});
}

export class ApiError extends Error {
    errors: Errors;

    constructor(errors: Errors) {
        super('ApiError');
        this.errors = errors;
    }
}