import queryString from "query-string";

export const apiUrl = (url: string): string => rootUrl(`api/${url}`);
export const rootUrl = (url: string): string => `${process.env.REACT_APP_API_URL}/${url}`;

export const formatUrl = (url: string, query?: any): string =>
  queryString.stringifyUrl({ url: url, query: query });
