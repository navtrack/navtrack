import classNames from "classnames";
import React, { ReactNode } from "react";

export type Props = {
  index: number;
  selectedIndex: number;
  length: number;
  children: ReactNode;
  onClick: () => void;
};

export default function ScrollableTableRow(props: Props) {
  return (
    <div
      className={classNames(
        "flex flex-row px-3 py-1 text-sm",
        props.index !== props.length - 1 ? "border-b" : "",
        props.index === props.selectedIndex ? "bg-gray-200" : ""
      )}
      id={`scrollableTableRow-${props.index}`}
      onClick={props.onClick}>
      {props.children}
    </div>
  );
}
