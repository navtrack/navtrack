import { subDays } from "date-fns";
import { DateRange } from "../locationFilterTypes";

export const dateOptions = [
  {
    range: DateRange.ThisWeek,
    name: "generic.this-week",
    startDate: subDays(new Date(), new Date().getDay() - 1),
    endDate: new Date()
  },
  {
    range: DateRange.LastWeek,
    name: "generic.last-week",
    startDate: subDays(new Date(), new Date().getDay() + 7),
    endDate: subDays(new Date(), new Date().getDay())
  },
  {
    range: DateRange.ThisMonth,
    name: "generic.this-month",
    startDate: new Date(new Date().getFullYear(), new Date().getMonth(), 1),
    endDate: new Date()
  },
  {
    range: DateRange.LastMonth,
    name: "generic.last-month",
    startDate: new Date(new Date().getFullYear(), new Date().getMonth() - 1, 1),
    endDate: new Date(new Date().getFullYear(), new Date().getMonth(), 0)
  },
  {
    range: DateRange.Custom,
    name: "generic.custom"
  }
];
