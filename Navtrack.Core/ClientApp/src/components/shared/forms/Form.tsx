import React, { ReactNode } from "react";

type Props = {
  title: string;
  children: ReactNode;
  actions: ReactNode;
  rightChildren?: ReactNode;
};

export default function Form(props: Props) {
  return (
    <div className="shadow rounded bg-white flex flex-col p-4">
      <div className="font-medium text-lg mb-4">
        <div className="border-b pb-4">{props.title}</div>
      </div>
      <div className="flex mb-4">
        <div className="mr-4 w-1/2">{props.children}</div>
        <div className="ml-4 w-1/2">{props.rightChildren}</div>
      </div>
      <div className="border-t pt-4">{props.actions}</div>
    </div>
  );
}
