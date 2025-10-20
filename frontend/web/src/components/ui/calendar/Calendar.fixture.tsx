import { useFixtureInput, useFixtureState } from "react-cosmos/client";
import { Calendar } from "./Calendar";
import { useState } from "react";

export default {
  Calendar: () => {
    const [value, setValue] = useFixtureInput("selectedDate", new Date());

    return <Calendar onChange={setValue} selectedDate={value} />;
  },
  Interval: () => {
    const [startDate, setStartDate] = useFixtureInput<Date | undefined>(
      "startDate",
      undefined
    );
    const [endDate, setEndDate] = useFixtureInput<Date | undefined>(
      "endDate",
      undefined
    );

    return (
      <Calendar
        range
        onChangeRange={(dates) => {
          setStartDate(dates[0]);
          setEndDate(dates[1]);
        }}
      />
    );
  }
};
