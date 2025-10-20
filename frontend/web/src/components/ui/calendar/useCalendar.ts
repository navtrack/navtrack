import { useCallback, useMemo, useState } from "react";
import {
  addDays,
  addMonths,
  isBefore,
  isWithinInterval,
  subMonths
} from "date-fns";
import { dateIsEqual } from "@navtrack/shared/utils/date";
import { useClose } from "@headlessui/react";

export type Day = {
  date: Date;
  isCurrentMonth: boolean;
  isSelected: boolean;
  isToday: boolean;
  isHoverInterval: boolean;
};

export type CalendarProps = {
  value?: Date;
  onChange?: (date: Date) => void;
  disabled?: boolean;
  label?: string;
  interval?: boolean;
  selectedDate?: Date;
  selectedInterval?: [Date, Date];
  onChangeInterval?: (dates: [Date, Date]) => void;
};

export function useCalendar(props: CalendarProps) {
  const close = useClose();
  const [selectedDate, setSelectedDate] = useState(
    !props.interval ? (props.selectedDate ?? new Date()) : undefined
  );
  const [startDate, setStartDate] = useState<Date | undefined>(
    props.selectedInterval ? props.selectedInterval[0] : undefined
  );
  const [endDate, setEndDate] = useState<Date | undefined>(
    props.selectedInterval ? props.selectedInterval[1] : undefined
  );
  const [hover, setHover] = useState<Date | undefined>(undefined);

  const [first, setFirst] = useState(true);

  const [currentMonth, setCurrentMonth] = useState(
    endDate ?? props.selectedDate ?? new Date()
  );

  const isSelected = useCallback(
    (date: Date) => {
      return (
        (selectedDate !== undefined && dateIsEqual(selectedDate, date)) ||
        (startDate !== undefined && dateIsEqual(startDate, date)) ||
        (startDate !== undefined &&
          endDate !== undefined &&
          isWithinInterval(date, {
            start: startDate,
            end: endDate
          }))
      );
    },
    [endDate, selectedDate, startDate]
  );

  const isHover = useCallback(
    (date: Date) => {
      return (
        startDate !== undefined &&
        endDate === undefined &&
        hover !== undefined &&
        isWithinInterval(date, {
          start: isBefore(hover, startDate) ? hover : startDate,
          end: isBefore(hover, startDate) ? startDate : hover
        })
      );
    },
    [endDate, hover, startDate]
  );

  const days: Day[] = useMemo(() => {
    const year = currentMonth.getFullYear();
    const month = currentMonth.getMonth();

    const firstOfMonth = new Date(year, month, 1);

    const dayOfWeek = (firstOfMonth.getDay() + 6) % 7;

    const startDate = new Date(firstOfMonth);
    startDate.setDate(firstOfMonth.getDate() - dayOfWeek);

    const days: Day[] = [];
    for (let i = 0; i < 42; i++) {
      const date = addDays(startDate, i);

      days.push({
        date,
        isCurrentMonth: date.getMonth() === month,
        isSelected: isSelected(date),
        isToday: dateIsEqual(date, new Date()),
        isHoverInterval: isHover(date)
      });
    }

    return days;
  }, [currentMonth, isHover, isSelected]);

  const selectDay = useCallback(
    (day: Day) => {
      if (props.interval) {
        if (first) {
          setStartDate(day.date);
          setEndDate(undefined);
          setFirst(false);
        } else if (startDate !== undefined) {
          close();
          const endDateIsBefore = isBefore(day.date, startDate);
          const from = endDateIsBefore ? day.date : startDate;
          const to = endDateIsBefore ? startDate : day.date;
          setStartDate(from);
          setEndDate(to);
          setFirst(true);
          props.onChangeInterval?.([from, to]);
          if (
            day.date.getMonth() !== currentMonth.getMonth() &&
            !endDateIsBefore
          ) {
            setCurrentMonth(day.date);
          }
        }
      } else {
        setSelectedDate(day.date);
        props.onChange?.(day.date);
        close();

        if (day.date.getMonth() !== currentMonth.getMonth()) {
          setCurrentMonth(day.date);
        }
      }
    },
    [close, currentMonth, first, props, startDate]
  );

  return {
    currentMonth,
    selectedDate,
    days,
    selectNextMonth: () => setCurrentMonth(addMonths(currentMonth, 1)),
    selectPreviousMonth: () => setCurrentMonth(subMonths(currentMonth, 1)),
    selectDay: selectDay,
    setHover: (day: Day) => setHover(day.date)
  };
}
