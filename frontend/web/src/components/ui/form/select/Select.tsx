import { FocusEventHandler } from "react";
import { InputError } from "../input-error/InputError";
import { c, classNames } from "@navtrack/shared/utils/tailwind";
import { InputLabel } from "../InputLabel";

export type SelectProps = {
  name?: string;
  label?: string;
  placeholder?: string;
  value?: string;
  options: SelectOption[];
  onChange?: (value: string) => void;
  onBlur?: FocusEventHandler<HTMLInputElement>;
  error?: string;
  disabled?: boolean;
  className?: string;
  isLoading?: boolean;
};

export type SelectOption = {
  value: string;
  label: string;
};

export function Select(props: SelectProps) {
  return (
    <div>
      <InputLabel name={props.name} label={props.label} />
      <div className="relative mt-1 flex rounded-md shadow-sm">
        <select
          name={props.name}
          className={classNames(
            "block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 focus:ring-2 focus:ring-inset focus:ring-indigo-600",
            c(props.error, "ring-red-600"),
            c(props.isLoading, "animate-pulse bg-gray-50"),
            props.className
          )}
          value={props.value}
          placeholder={props.placeholder}
          disabled={props.disabled || props.isLoading}
          onChange={(e) => {
            const item = props.options.find((x) => x.value === e.target.value);
            if (item) {
              props.onChange?.(item.value);
            }
          }}>
          {!props.isLoading &&
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
