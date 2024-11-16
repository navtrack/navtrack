import { FocusEventHandler } from "react";
import { InputError } from "../input-error/InputError";
import { c, classNames } from "@navtrack/shared/utils/tailwind";
import { InputLabel } from "../InputLabel";

export type SelectProps = {
  name?: string;
  label?: string;
  placeholder?: string;
  value?: string;
  values?: string[];
  options: SelectOption[];
  onChange?: (value: string) => void;
  onChangeMultiple?: (value: string[]) => void;
  onBlur?: FocusEventHandler<HTMLInputElement>;
  error?: string;
  disabled?: boolean;
  className?: string;
  loading?: boolean;
  size?: number;
  multiple?: boolean;
};

export type SelectOption = {
  value: string;
  label: string;
};

export function Select(props: SelectProps) {
  return (
    <div>
      <InputLabel name={props.name} label={props.label} />
      <div className="relative flex rounded-md shadow-sm">
        <select
          size={props.size}
          name={props.name}
          multiple={props.multiple}
          className={classNames(
            "block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 focus:ring-2 focus:ring-inset focus:ring-blue-700",
            c(props.error, "ring-red-600"),
            c(props.loading, "animate-pulse bg-gray-50"),
            props.className
          )}
          value={props.values || props.value}
          //placeholder={props.placeholder} TODO
          disabled={props.disabled || props.loading}
          onChange={(e) => {
            console.log(e.target.value);
            const item = props.options.find((x) => x.value === e.target.value);
            if (item) {
              props.onChange?.(item.value);
            }

            if (e.target.selectedOptions) {
              const options = [...e.target.selectedOptions];
              const values = options.map((option) => option.value);
              props.onChangeMultiple?.(values);
            }
          }}>
          {!props.loading &&
            props.options.map((x) => (
              <option key={x.value} value={x.value}>
                {x.label}
              </option>
            ))}
        </select>
      </div>
      <InputError error={props.error} />
    </div>
  );
}
