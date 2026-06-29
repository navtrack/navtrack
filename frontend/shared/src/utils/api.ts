import { format } from "date-fns";

export function formatApiDate(date?: Date) {
  return date ? format(date, "yyyy-MM-dd") : undefined;
}
