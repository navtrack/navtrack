import {
  ChangeEventHandler,
  FocusEventHandler,
  MouseEventHandler,
  ReactNode
} from "react";
import { InputError } from "../input-error/InputError";
import { c, classNames } from "@navtrack/shared/utils/tailwind";
import { InputLabel } from "../InputLabel";

export type TextInputProps = {
  label?: string;
  placeholder?: string;
  type?: "text" | "password" | "number";
  name?: string;
  value?: string | number;
  disabled?: boolean;
  readOnly?: boolean;
  className?: string;
  leftAddon?: ReactNode;
  rightAddon?: ReactNode;
  error?: string;
  autoComplete?: "off" | string;
  size?: "xs" | "sm";
  loading?: boolean;
  onChange?: ChangeEventHandler<HTMLInputElement>;
  onBlur?: FocusEventHandler<HTMLInputElement>;
  onClick?: MouseEventHandler<HTMLInputElement>;
  onMouseEnter?: MouseEventHandler<HTMLInputElement>;
  onMouseLeave?: MouseEventHandler<HTMLInputElement>;
};

export function TextInput(props: TextInputProps) {
  return (
    <div>
      <InputLabel name={props.name} label={props.label} />
      <div className="relative flex">
        {props.leftAddon}
        <input
          name={props.name}
          type={props.type ?? "text"}
          className={classNames(
            "px-2 block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-blue-500",
            c(props.size === "sm", "py-1 text-sm"),
            c(props.size === "xs", "py-1 text-xs"),
            c(!!props.error, "ring-red-600"),
            c(props.loading, "animate-pulse bg-gray-100"),
            c(props.disabled, "bg-gray-100 text-gray-400"),
            props.className
          )}
          value={props.value}
          onChange={props.onChange}
          onBlur={props.onBlur}
          onClick={props.onClick}
          onMouseEnter={props.onMouseEnter}
          onMouseLeave={props.onMouseLeave}
          placeholder={props.placeholder}
          readOnly={props.readOnly || props.loading}
          disabled={props.disabled || props.loading}
          autoComplete={props.autoComplete}
        />
        {props.rightAddon}
      </div>
      <InputError error={props.error} />
    </div>
  );
}
