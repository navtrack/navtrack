import React, { ReactNode } from "react";
import InputError from "components/common/InputError";
import { AppError } from "services/httpClient/AppError";
import classNames from "classnames";
import Dropdown from "./Dropdown";

type Props = {
  name: string;
  value: string | number | string[];
  onChange: (event: React.ChangeEvent<HTMLSelectElement>) => void;
  errorKey?: string;
  error?: AppError | undefined;
  className?: string;
  children: ReactNode;
  tip?: string;
};

export default function DropdownInput(props: Props) {
  return (
    <div className={classNames("flex flex-row", props.className)}>
      <div className="w-20 text-gray-700 font-medium h-10 flex items-center text-sm">
        {props.name}
      </div>
      <div className="w-20 text-gray-700 font-medium w-5/12">
        <Dropdown value={props.value} onChange={props.onChange}>
          {props.children}
        </Dropdown>
        {props.errorKey && <InputError name={props.errorKey} error={props.error} />}
      </div>
      {props.tip && (
        <div className="ml-4 text-gray-700 text-sm h-10 flex items-center">{props.tip}</div>
      )}
    </div>
  );
}
