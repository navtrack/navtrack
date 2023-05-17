import styled from "@mui/styled-engine";
import {
  ChangeEventHandler,
  FocusEventHandler,
  MouseEventHandler,
  ReactNode,
  RefObject
} from "react";
import { InputError } from "../input/InputError";
import { c, classNames } from "@navtrack/shared/utils/tailwind";

export type TextInputProps = {
  disabled?: boolean;
  leftAddon?: ReactNode;
  rightAddon?: ReactNode;
  className?: string;
  label?: string;
  placeholder?: string;
  type?: "text" | "password" | "number";
  name?: string;
  value?: string | number;
  onChange?: ChangeEventHandler<HTMLInputElement>;
  onBlur?: FocusEventHandler<HTMLInputElement>;
  onClick?: MouseEventHandler<HTMLInputElement>;
  onMouseEnter?: MouseEventHandler<HTMLInputElement>;
  onMouseLeave?: MouseEventHandler<HTMLInputElement>;
  readOnly?: boolean;
  error?: string;
  ref?: RefObject<HTMLInputElement>;
  hideErrorMessage?: boolean;
  autoCompleteOff?: boolean;
};

const StyledInput = styled("input")`
  ::-webkit-outer-spin-button,
  ::-webkit-inner-spin-button {
    -webkit-appearance: none;
    margin: 0;
  }
  -moz-appearance: textfield;
`;

export function TextInput(props: TextInputProps) {
  return (
    <div>
      <label
        htmlFor={props.name}
        className="block text-sm font-medium text-gray-700">
        {props.label}
      </label>
      <div className="relative mt-1 flex rounded-md shadow-sm">
        {props.leftAddon}
        <StyledInput
          ref={props.ref}
          disabled={props.disabled}
          name={props.name}
          type={props.type ?? "text"}
          className={classNames(
            "w-full rounded-md border border-gray-300 px-3 py-1.5 text-sm text-gray-700 placeholder-gray-400 shadow-sm",
            c(!!props.error, "border-red-600"),
            props.className
          )}
          value={props.value}
          onChange={props.onChange}
          onBlur={props.onBlur}
          onClick={props.onClick}
          onMouseEnter={props.onMouseEnter}
          onMouseLeave={props.onMouseLeave}
          placeholder={props.placeholder}
          readOnly={props.readOnly}
          autoComplete={props.autoCompleteOff ? "off" : undefined}
        />
        {props.rightAddon}
      </div>
      {!props.hideErrorMessage && <InputError error={props.error} />}
    </div>
  );
}
