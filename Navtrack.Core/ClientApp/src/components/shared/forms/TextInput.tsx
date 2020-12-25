import classNames from "classnames";
import { useFormikContext } from "formik";
import React, { ReactNode } from "react";
import { PropertyValidationResult } from "../../../services/validation/PropertyValidationResult";
import InputWrapper from "./InputWrapper";

type Props = {
  name?: string; // TODO make mandatory
  title?: string;
  value: string | number | string[];
  onChange?: (event: React.ChangeEvent<HTMLInputElement>) => void;
  validationResult?: PropertyValidationResult | undefined;
  className?: string;
  placeholder?: string;
  type?: "text" | "password" | "number";
  children?: ReactNode;
  onClick?: (event: React.MouseEvent<HTMLInputElement, MouseEvent>) => void;
  readOnly?: boolean;
  size?: "lg";
};

export default function TextInput(props: Props) {
  const formikContext = useFormikContext();
  const fieldMeta = formikContext.getFieldMeta(props.name ? props.name : ""); // TODO make mandatory .name

  return (
    <InputWrapper title={props.title} validationResult={props.validationResult} name={props.name}>
      <input
        name={props.name}
        type={props.type}
        className={classNames(
          "border-gray-300 border bg-gray-100 rounded-md text-gray-700 w-full",
          props.size === "lg" ? "text-md py-2 px-4" : "text-sm py-1 px-3 ",
          { "border-red-600": fieldMeta.error && fieldMeta.touched }
        )}
        style={{
          WebkitAppearance: "none"
        }}
        value={props.value}
        onChange={formikContext.handleChange}
        onBlur={formikContext.handleBlur}
        placeholder={props.placeholder}
        readOnly={props.readOnly}
        onClick={props.onClick}
      />
      {props.children && <div className="text-sm ml-2 flex items-center">{props.children}</div>}
    </InputWrapper>
  );
}
