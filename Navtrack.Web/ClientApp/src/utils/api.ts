import { ErrorModel } from "../api/model/generated";

export function getError(error: any) {
  const response = error.response.data as ErrorModel;

  return response;
}
