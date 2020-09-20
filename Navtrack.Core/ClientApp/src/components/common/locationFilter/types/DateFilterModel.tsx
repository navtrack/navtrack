import moment, { Moment } from "moment";
import Configuration from "../../../../services/Configuration";
import { DateFilterType } from "./DateFilterType";
import { NumberFilterType } from "./NumberFilterType";

export type DateFilterModel = {
  singleDate: Moment;
  intervalStartDate: Moment;
  intervalEndDate: Moment;
  dateFilterType: DateFilterType;
  numberFilterType: NumberFilterType;
  startDate: Moment;
  endDate: Moment;
};

export const DefaultDateFilterModel: DateFilterModel = {
  singleDate: moment(),
  intervalStartDate: moment(),
  intervalEndDate: moment(),
  dateFilterType: DateFilterType.Last7Days,
  numberFilterType: NumberFilterType.Interval,
  startDate: moment().subtract(10, "days"),
  endDate: moment()
};

export const dateFilterToString = (dateFilter: DateFilterModel) => {
  if (dateFilter.numberFilterType === NumberFilterType.Single) {
    return dateFilter.singleDate.format(Configuration.dateFormatAlt);
  } else {
    if (dateFilter.startDate.isSame(dateFilter.endDate, "day")) {
      return dateFilter.startDate.format(Configuration.dateFormatAlt);
    }

    return `${dateFilter.startDate.format(
      Configuration.dateFormatAlt
    )} - ${dateFilter.endDate.format(Configuration.dateFormatAlt)}`;
  }
};
