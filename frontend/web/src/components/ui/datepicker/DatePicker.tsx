import { Datepicker } from "flowbite-react";
import { InputLabel } from "../form/InputLabel";

export type DatePickerProps = {
  value?: Date;
  onChange: (date: Date) => void;
  disabled?: boolean;
  label?: string;
};

export function DatePicker(props: DatePickerProps) {
  return (
    <div>
      <InputLabel label={props.label} />
      <Datepicker
        defaultDate={props.value}
        disabled={props.disabled}
        onSelectedDateChanged={(date) => props.onChange(date)}
        theme={{
          root: {
            input: {
              field: {
                input: {
                  sizes: {
                    sm: "p-2 sm:text-xs",
                    md: "p-1.5 text-sm",
                    lg: "sm:text-md p-4"
                  },
                  colors: {
                    gray: "bg-gray-50 border-gray-300 text-gray-900 focus:border-blue-500 focus:ring-blue-500 dark:border-gray-600 dark:bg-gray-700 dark:text-white dark:placeholder-gray-400 dark:focus:border-blue-500 dark:focus:ring-blue-500"
                  }
                }
              }
            }
          },
          popup: {
            footer: {
              button: {
                today:
                  "bg-blue-700 text-white hover:bg-blue-800 focus:ring-0 rounded-md"
              }
            }
          },
          views: {
            days: {
              items: {
                item: {
                  selected: "bg-blue-700 text-white hover:bg-blue-500"
                }
              }
            }
          }
        }}
      />
    </div>
  );
}
