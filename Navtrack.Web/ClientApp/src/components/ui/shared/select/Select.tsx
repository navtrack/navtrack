import classNames from "classnames";
import { FocusEventHandler } from "react";
import InputError from "../input/InputError";
import { ISelectOption } from "./types";

export interface ISelect {
  name?: string;
  label?: string;
  placeholder?: string;
  value?: string;
  options: ISelectOption[];
  onChange?: (value: string) => void;
  onBlur?: FocusEventHandler<HTMLInputElement>;
  error?: string;
  disabled?: boolean;
  className?: string;
  readOnly?: boolean;
  hideErrorMessage?: boolean;
  autoCompleteOff?: boolean;
}

export default function Select(props: ISelect) {
  return (
    <div>
      <label
        htmlFor={props.name}
        className="block text-sm font-medium text-gray-700">
        {props.label}
      </label>
      <div className="mt-1 relative rounded-md shadow-sm flex">
        <select
          disabled={props.disabled}
          name={props.name}
          className={classNames(
            "border-gray-300 border rounded-md text-gray-700 w-full text-sm py-1.5 px-3 shadow-sm",
            { "border-red-600": props.error },
            props.className
          )}
          value={props.value}
          placeholder={props.placeholder}
          onChange={(e) => {
            const item = props.options.find((x) => x.value === e.target.value);
            if (item) {
              props.onChange?.(item.value);
            }
          }}>
          {props.options.map((x) => (
            <option key={x.value} value={x.value}>
              {x.label}
            </option>
          ))}
        </select>
      </div>
      {!props.hideErrorMessage && <InputError error={props.error} />}
    </div>
  );
}
