import React, { ReactNode } from "react";

type Props = {
  title: string;
  children: ReactNode;
  actions: ReactNode;
};

export default function Form(props: Props) {
  return (
    <div className="shadow rounded bg-white flex flex-col">
      <div className="w">
        <div className="font-medium text-lg px-4">
          <div className="border-b py-4">{props.title}</div>
        </div>
        <div className="p-4 w-1/2">{props.children}</div>
        <div className="px-4">
          <div className="border-t py-4">{props.actions}</div>
        </div>
      </div>
    </div>
  );
}
