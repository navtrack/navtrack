import { ApiUtil } from "./ApiUtil";

export const Api = {
    get: <T>(url: string) => {
        return fetch(ApiUtil.apiUrl(url), {
            method: "get"
        }).then(handleResponse<T>())
    },

    post: <T>(url: string, bodyObject: any): Promise<T> => {
        return fetch(ApiUtil.apiUrl(url), {
            method: "post",
            headers: {
                "Content-Type": "application/json-patch+json",
            },
            body: JSON.stringify(bodyObject)
        }).then(handleResponse<T>());
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