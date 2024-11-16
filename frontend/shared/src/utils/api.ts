import { Error } from "../api/model/generated";

export const getError = (error: any) => {
  const response = error.response.data as Error;

  return response;
};
