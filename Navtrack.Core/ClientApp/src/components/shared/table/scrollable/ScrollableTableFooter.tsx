import React, { ReactNode } from "react";

export type Props = {
  children: ReactNode;
};

export default function ScrollableTableFooter(props: Props) {
  return (
    <div className="flex flex-row px-3 py-1 text-xs bg-gray-100 border-t rounded-b">
      {props.children}
    </div>
  );
}
