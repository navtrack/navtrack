import { HttpClientUtil } from "./HttpClientUtil";

export const HttpClient = {
    get: <T>(url: string) => {
        return fetch(HttpClientUtil.apiUrl(url), {
            method: "GET"
        }).then(handleResponse<T>())
    },

    post: (url: string, bodyObject: any): Promise<Response> => {
        return fetch(HttpClientUtil.apiUrl(url), {
            method: "POST",
            headers: {
                "Content-Type": "application/json-patch+json",
            },
            body: JSON.stringify(bodyObject)
        }).then(handleResponse<Response>());
    },

    put: (url: string, bodyObject: any): Promise<Response> => {
        return fetch(HttpClientUtil.apiUrl(url), {
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
            throw json;
        }

        return json as T;
    };
}