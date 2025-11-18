import { startOfWeek, subDays, subWeeks } from "date-fns";
import { DateRange } from "../locationFilterTypes";

export const dateOptions = [
  {
    range: DateRange.ThisWeek,
    name: "generic.this-week",
    startDate: startOfWeek(new Date(), { weekStartsOn: 1 }),
    endDate: new Date()
  },
  {
    range: DateRange.LastWeek,
    name: "generic.last-week",
    startDate: subWeeks(startOfWeek(new Date(), { weekStartsOn: 1 }), 1),
    endDate: subDays(startOfWeek(new Date(), { weekStartsOn: 1 }), 1)
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
