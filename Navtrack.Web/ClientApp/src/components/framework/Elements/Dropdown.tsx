import React, { ReactNode } from "react";

type Props = {
  value: string | number | string[];
  onChange: (event: React.ChangeEvent<HTMLSelectElement>) => void;
  children: ReactNode;
};

export default function Dropdown(props: Props) {
  return (
    <div className="relative shadow rounded w-full">
      <select
        className="block appearance-none bg-white px-3 py-2 cursor-pointer focus:outline-none bg-gray-200 w-full"
        value={props.value}
        onChange={props.onChange}>
        {props.children}
      </select>
      <div className="pointer-events-none absolute inset-y-0 right-0 flex items-center px-2 pt-1">
        <i className="fas fa-chevron-down" />
      </div>
    </div>
  );
}
