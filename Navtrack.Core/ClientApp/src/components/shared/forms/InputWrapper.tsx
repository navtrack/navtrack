import classNames from "classnames";
import React, { ReactNode } from "react";
import { FormattedMessage } from "react-intl";
import { PropertyValidationResult } from "../../../services/validation/PropertyValidationResult";
import InputError from "./InputError";

type Props = {
  name?: string; // TODO: make mandatory
  title?: string;
  children: ReactNode;
  validationResult: PropertyValidationResult | undefined;
  className?: string;
  placeHolder?: string;
};

export default function InputWrapper(props: Props) {
  return (
    <div className={classNames("mb-4 text-base font-medium text-gray-700", props.className)}>
      {props.title && (
        <div className="mb-1 text-sm">
          <FormattedMessage id={props.title} />
        </div>
      )}
      <div className="flex">{props.children}</div>
      <InputError name={props.name} />
    </div>
  );
}
