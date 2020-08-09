import queryString from "query-string";

export const apiUrl = (url: string): string => `${process.env.REACT_APP_API_URL}/api/${url}`;
export const identityUrl = (url: string): string => `${process.env.REACT_APP_API_URL}/${url}`;

export const formatUrl = (url: string, query?: any): string =>
  queryString.stringifyUrl({ url: url, query: query });
