import { format } from "date-fns";
import { ErrorModel } from "../api/model";

export function getError(error: any) {
  const response = error.response.data as ErrorModel;

  return response;
}

export function formatApiDate(date?: Date) {
  return date ? format(date, "yyyy-MM-dd") : undefined;
}
