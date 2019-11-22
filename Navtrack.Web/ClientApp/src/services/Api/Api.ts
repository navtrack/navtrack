import { ApiUtil } from "./ApiUtil";

export const Api = {
    get: <T>(url: string) => {
        return fetch(ApiUtil.apiUrl(url), {
            method: "get"
        }).then(handleResponse<T>())
    },

    post: (url: string, bodyObject: any): Promise<Response> => {
        return fetch(ApiUtil.apiUrl(url), {
            method: "post",
            headers: {
                "Content-Type": "application/json-patch+json",
            },
            body: JSON.stringify(bodyObject)
        });
    },

    put: (url: string, bodyObject: any): Promise<Response> => {
        return fetch(ApiUtil.apiUrl(url), {
            method: "PUT",
            headers: {
                "Content-Type": "application/json-patch+json",
            },
            body: JSON.stringify(bodyObject)
        });
    }
};

function handleResponse<T>(): (value: Response) => Promise<T> {
    return async (response) => {
        const json = await response.json();

        if (!response.ok) {
            const apiError = { ...json, error: response.status };

            throw apiError;
        }

        return json as T;
    };
}