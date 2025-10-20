import { InputLabel } from "../form/InputLabel";
import { Popover, PopoverButton, PopoverPanel } from "@headlessui/react";
import { formatDate } from "date-fns";

import { Calendar } from "../calendar/Calendar";

export type DatePickerProps = {
  value: Date;
  onChange: (date: Date) => void;
  disabled?: boolean;
  label?: string;
  range?: boolean;
};

export function DatePicker(props: DatePickerProps) {
  return (
    <div>
      <InputLabel label={props.label} />
      <Popover>
        <PopoverButton className="bg-white hover:cursor-pointer hover:bg-gray-100 px-3 rounded-md py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400">
          {formatDate(props.value, "yyyy-MM-dd")}
        </PopoverButton>
        <PopoverPanel transition anchor="bottom start">
          <div className="border border-gray-200 mt-2 bg-white rounded-md p-4 w-100">
            <Calendar selectedDate={props.value} onChange={props.onChange} />
          </div>
        </PopoverPanel>
      </Popover>
    </div>
  );
}
