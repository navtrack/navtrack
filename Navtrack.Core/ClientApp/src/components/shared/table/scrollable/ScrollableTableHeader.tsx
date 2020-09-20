import React, { ReactNode } from "react";

export type Props = {
  children: ReactNode;
};

export default function ScrollableTableHeader(props: Props) {
  return (
    <div className="flex flex-row px-3 py-1 text-xs uppercase font-semibold bg-gray-100 border-t border-b">
      {props.children}
    </div>
  );
}
