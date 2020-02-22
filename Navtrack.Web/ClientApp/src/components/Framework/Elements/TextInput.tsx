import React from "react";
import InputError from "components/Common/InputError";
import { AppError } from "services/HttpClient/AppError";
import classNames from "classnames";

type Props = {
  name: string,
  value: string | number | string[],
  onChange: (event: React.ChangeEvent<HTMLInputElement>) => void,
  errorKey: string,
  error: AppError | undefined,
  className?: string
}

export default function TextInput(props: Props) {
  return (
    <div className={classNames("flex flex-row mb-3", props.className)}>
      <div className="w-20 text-gray-700 font-medium h-10 flex items-center text-sm">{props.name}</div>
      <div className="text-gray-700 font-medium w-5/12">
        <input className="shadow bg-gray-200 appearance-none rounded py-2 px-3 text-gray-700 focus:outline-none focus:border focus:border-gray-900 w-full"
          value={props.value}
          onChange={props.onChange}
          placeholder={props.name}
        />
        <InputError name={props.errorKey} error={props.error} />
      </div>
    </div>
  );
};