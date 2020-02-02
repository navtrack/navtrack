import React, { } from "react";
import { AppError } from "../../services/HttpClient/AppError";
import { getParameterCaseInsensitive } from "../../services/Util/ObjectUtil";

type Props = {
  name: string,
  errors: AppError | undefined
}

export default function InputError(props: Props) {
  const errors: string[] = getParameterCaseInsensitive(props.errors && props.errors.validationResult, props.name);
  return (
    <>
      {errors && errors.map((x, i) =>
        <p className="text-red-500 text-xs italic mt-2" key={i}>{x}</p>)}
    </>
  );
}

export const AddError = <T extends {}>(validationResult: Record<keyof T, string[]>, key: keyof T, message: string) => {
  if (key in validationResult) {
    validationResult[key].push(message);
  } else {
    validationResult[key] = [message];
  }
};


export const ClearError = <T extends {}>(appError: AppError | undefined, key: keyof T) => {
  if (appError) {
    return new AppError({ ...appError.validationResult, key: [] });
  }

  return new AppError({ key: [] });
};


export const HasErrors = (errors: Record<string, string[]>): boolean => {
  return Object.keys(errors).length > 0;
};

