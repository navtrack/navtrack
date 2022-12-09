import { ErrorModel } from "../api/model/generated/errorModel";

export const getError = (error: any) => {
  const response = error.response.data as ErrorModel;

  return response;
};
