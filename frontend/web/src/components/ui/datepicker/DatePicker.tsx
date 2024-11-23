import { Datepicker } from "flowbite-react";
import { InputLabel } from "../form/InputLabel";
import { useIntl } from "react-intl";

export type DatePickerProps = {
  value?: Date;
  onChange: (date: Date) => void;
  disabled?: boolean;
  label?: string;
};

export function DatePicker(props: DatePickerProps) {
  const intl = useIntl();

  return (
    <div>
      <InputLabel label={props.label} />
      <Datepicker
        labelClearButton={intl.formatMessage({ id: "generic.close" })}
        value={props.value}
        disabled={props.disabled}
        onChange={(date) => (date !== null ? props.onChange(date) : undefined)}
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
                  },
                  withIcon: {
                    on: "pl-10"
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
                  selected: "bg-blue-700 text-white hover:bg-blue-600",
                  disabled: "text-gray-500"
                }
              }
            },
            months: {
              items: {
                item: {
                  selected: "bg-blue-700 text-white hover:bg-blue-600"
                }
              }
            },
            years: {
              items: {
                item: {
                  selected: "bg-blue-700 text-white hover:bg-blue-600"
                }
              }
            },
            decades: {
              items: {
                item: {
                  selected: "bg-blue-700 text-white hover:bg-blue-600"
                }
              }
            }
          }
        }}
      />
    </div>
  );
}
