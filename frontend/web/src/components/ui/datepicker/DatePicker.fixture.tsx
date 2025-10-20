import { DatePicker } from "./DatePicker";
import { useFixtureInput } from "react-cosmos/client";
import { DateRangePicker } from "./DateRangePicker";
import { addDays } from "date-fns";

export default {
  Default: () => {
    const [value, setValue] = useFixtureInput("selectedDate", new Date());

    return <DatePicker value={value} onChange={setValue} />;
  },
  Range: () => {
    const [startDate, setStartDate] = useFixtureInput<Date>(
      "startDate",
      new Date()
    );
    const [endDate, setEndDate] = useFixtureInput<Date>(
      "endDate",
      addDays(new Date(), 7)
    );

    return (
      <DateRangePicker
        value={[startDate, endDate]}
        onChange={(dates) => {
          setStartDate(dates[0]);
          setEndDate(dates[1]);
        }}
      />
    );
  }
};
