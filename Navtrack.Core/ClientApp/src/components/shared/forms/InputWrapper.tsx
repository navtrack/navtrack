import classNames from "classnames";
import React, { ReactNode } from "react";
import { PropertyValidationResult } from "../../../services/validation/PropertyValidationResult";
import InputError from "./InputError";

type Props = {
  name?: string;
  children: ReactNode;
  validationResult: PropertyValidationResult | undefined;
  className?: string;
  placeHolder?: string;
};

export default function InputWrapper(props: Props) {
  return (
    <div className={classNames("mb-3 text-base font-medium text-gray-700", props.className)}>
      {props.name && <div className="mb-1 text-sm">{props.name}</div>}
      <div className="flex">{props.children}</div>
      <InputError validationResult={props.validationResult} />
    </div>
  );
}
