import { parseISO } from "date-fns";

export function useConvert2() {
  const stringToDate = (dateTime: string) => {
    return parseISO(dateTime);
  };

  return {
    stringToDate
  };
}
