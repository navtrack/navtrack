import { classNames } from "@navtrack/shared/utils/tailwind";
import { ReactNode } from "react";

type AlertProps = {
  className?: string;
  children?: ReactNode;
};

export function Alert(props: AlertProps) {
  return (
    <div
      className={classNames(
        "rounded-md bg-red-100 px-4 py-3 text-sm text-red-700",
        props.className
      )}>
      {props.children}
    </div>
  );
}
