import { InputLabel } from "../form/InputLabel";
import { useState } from "react";
import {
  Dialog,
  DialogPanel,
  DialogTitle,
  Description,
  Popover,
  PopoverButton,
  PopoverPanel
} from "@headlessui/react";
import { formatDate } from "date-fns";

import { Calendar } from "../calendar/Calendar";

export type DateRangePickerProps = {
  value: [Date, Date];
  onChange?: (dates: [Date, Date]) => void;
  disabled?: boolean;
  label?: string;
};

export function DateRangePicker(props: DateRangePickerProps) {
  return (
    <div>
      <InputLabel label={props.label} />
      <Popover>
        <PopoverButton className="flex bg-white hover:cursor-pointer hover:bg-gray-100 px-3 rounded-md py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400">
          <div>{formatDate(props.value[0], "yyyy-MM-dd")}</div>
          <div className="mx-2">-</div>
          <div>{formatDate(props.value[1], "yyyy-MM-dd")}</div>
        </PopoverButton>
        <PopoverPanel transition anchor="bottom start">
          <div className="border border-gray-200 bg-white rounded-md p-4 w-100">
            <Calendar
              selectedRange={props.value}
              onChangeRange={props.onChange}
              range
            />
          </div>
        </PopoverPanel>
      </Popover>
    </div>
  );
}
