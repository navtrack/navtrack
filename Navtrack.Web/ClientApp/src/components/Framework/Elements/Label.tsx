import React, { ReactNode } from "react";
import classNames from "classnames";

type Props = {
  children: ReactNode,
  className?: string
}

export default function Label(props: Props) {
  return (
    <div className={classNames("text-gray-800 font-medium", props.className)}>
      {props.children}
    </div>
  );
};