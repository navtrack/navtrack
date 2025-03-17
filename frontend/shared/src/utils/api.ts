import { Error } from "../api/model";

export const getError = (error: any) => {
  const response = error.response.data as Error;

  return response;
};
