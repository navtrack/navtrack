export const apiUrl = (path: string): string => `${process.env.REACT_APP_API_URL}/api/${path}`;
export const identityUrl = (path: string): string => `${process.env.REACT_APP_API_URL}/${path}`;
