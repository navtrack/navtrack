import { ErrorModel } from "../api/model";

export function getError(error: any) {
  const response = error.response.data as ErrorModel;

  return response;
}
