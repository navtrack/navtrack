import React from "react";
import { PropertyValidationResult } from "framework/validation/PropertyValidationResult";

type Props = {
  validationResult: PropertyValidationResult | undefined;
  name?: string;
  error?: any;
};

export default function InputError(props: Props) {
  return (
    <>
      {props.validationResult &&
        props.validationResult.errors.map((x, i) => (
          <p className="text-red-600 text-xs italic mt-1" key={i}>
            {x}
          </p>
        ))}
    </>
  );
}
