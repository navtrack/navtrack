import classNames from "classnames";
import { ReactNode } from "react";

interface IAlert {
  className?: string;
  children?: ReactNode;
}

export default function Alert(props: IAlert) {
  return (
    <div
      className={classNames(
        "bg-red-100 px-4 py-3 text-sm text-red-700 rounded-md",
        props.className
      )}>
      {props.children}
    </div>
  );
}
