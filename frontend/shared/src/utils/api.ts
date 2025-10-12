import { ErrorModel } from "../api/model";

export const getError = (error: any) => {
  const response = error.response.data as ErrorModel;

  return response;
};
