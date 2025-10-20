import { Icon } from "../icon/Icon";
import {
  faChevronLeft,
  faChevronRight
} from "@fortawesome/free-solid-svg-icons";
import { useCalendar } from "./useCalendar";
import { format } from "date-fns";
import { c, classNames } from "@navtrack/shared/utils/tailwind";
import { useClose } from "@headlessui/react";

type CalendarProps = {
  selectedDate?: Date;
  selectedRange?: [Date, Date];
  range?: boolean;
  onChange?: (date: Date) => void;
  onChangeRange?: (dates: [Date, Date]) => void;
};

export function Calendar(props: CalendarProps) {
  const calendar = useCalendar({
    selectedDate: props.selectedDate,
    selectedInterval: props.selectedRange,
    interval: props.range,
    onChange: props.onChange,
    onChangeInterval: props.onChangeRange
  });

  return (
    <div>
      <div className="text-center">
        <div className="flex items-center text-gray-900">
          <button
            type="button"
            className="text-gray-400 hover:text-gray-500 hover:cursor-pointer"
            onClick={calendar.selectPreviousMonth}>
            <Icon icon={faChevronLeft} />
          </button>
          <div className="flex-auto text-sm font-semibold">
            {format(calendar.currentMonth, "MMMM yyyy")}
          </div>
          <button
            type="button"
            className="text-gray-400 hover:text-gray-500 hover:cursor-pointer"
            onClick={calendar.selectNextMonth}>
            <Icon icon={faChevronRight} />
          </button>
        </div>
        <div className="mt-6 grid grid-cols-7 text-xs/6 text-gray-500">
          <div>M</div>
          <div>T</div>
          <div>W</div>
          <div>T</div>
          <div>F</div>
          <div>S</div>
          <div>S</div>
        </div>
        <div className="isolate mt-2 grid grid-cols-7 gap-px  bg-gray-200 text-sm ring-1 ring-gray-200">
          {calendar.days.map((day) => (
            <button
              key={day.date.toISOString()}
              type="button"
              data-is-today={day.isToday ? "" : undefined}
              data-is-selected={day.isSelected ? "" : undefined}
              data-is-current-month={day.isCurrentMonth ? "" : undefined}
              data-is-hover-interval={day.isHoverInterval ? "" : undefined}
              onClick={() => calendar.selectDay(day)}
              onMouseEnter={() => calendar.setHover(day)}
              className={classNames(
                "hover:cursor-pointer py-1.5",
                c(
                  day.isCurrentMonth,
                  "bg-white hover:bg-gray-100 text-gray-900",
                  "bg-gray-50 text-gray-400"
                )
              )}>
              <div
                className={classNames(
                  "mx-auto flex size-7 items-center justify-center rounded-full",
                  c(day.isHoverInterval && !day.isToday, "bg-gray-500"),
                  c(
                    day.isHoverInterval && day.isToday,
                    "bg-blue-600 text-white"
                  ),
                  c(day.isHoverInterval && !day.isToday, "text-white"),
                  c(day.isSelected && !day.isToday, "bg-gray-800 text-white"),
                  c(day.isSelected && day.isToday, "bg-blue-800 text-white"),
                  c(day.isToday && !day.isHoverInterval, "text-blue-700"),
                  c(day.isToday || day.isSelected, "font-semibold")
                )}>
                {format(day.date, "d")}
              </div>
            </button>
          ))}
        </div>
      </div>
    </div>
  );
}
