import { parseISO } from "date-fns";

export function useConvert() {
  const stringToDate = (dateTime: string) => {
    return parseISO(dateTime);
  };

  return {
    stringToDate
  };
}
