import { subDays, subMonths } from "date-fns";
import { DateRange } from "../locationFilterTypes";

export const dateOptions = [
  {
    range: DateRange.Last7Days,
    name: "locations.filter.date.last-7-days",
    startDate: subDays(new Date(), 7),
    endDate: new Date()
  },
  {
    range: DateRange.Last14Days,
    name: "locations.filter.date.last-14-days",
    startDate: subDays(new Date(), 14),
    endDate: new Date()
  },
  {
    range: DateRange.Last28Days,
    name: "locations.filter.date.last-28-days",
    startDate: subDays(new Date(), 28),
    endDate: new Date()
  },
  {
    range: DateRange.Last3Months,
    name: "locations.filter.date.last-3-months",
    startDate: subMonths(new Date(), 3),
    endDate: new Date()
  },
  {
    range: DateRange.Last6Months,
    name: "locations.filter.date.last-6-months",
    startDate: subMonths(new Date(), 6),
    endDate: new Date()
  },
  {
    range: DateRange.Last12Months,
    name: "locations.filter.date.last-12-months",
    startDate: subMonths(new Date(), 12),
    endDate: new Date()
  },
  {
    range: DateRange.Custom,
    name: "generic.custom"
  }
];
