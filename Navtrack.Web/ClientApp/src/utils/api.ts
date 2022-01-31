import { ErrorModel } from "@navtrack/navtrack-shared/dist/api/model/generated";

export function getError(error: any) {
  const response = error.response.data as ErrorModel;

  return response;
}
