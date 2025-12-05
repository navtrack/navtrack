import { format } from "date-fns";
import { ErrorModel } from "../api/model";

export const getError = (error: any) => {
  const response = error.response.data as ErrorModel;

  return response;
};

export const formatApiDate = (date?: Date) => {
  return date ? format(date, "yyyy-MM-dd") : undefined;
};
