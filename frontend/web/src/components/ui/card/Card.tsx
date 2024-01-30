import { classNames } from "@navtrack/shared/utils/tailwind";
import { ReactNode } from "react";

export type CardProps = {
  children: ReactNode;
  className?: string;
};

export function Card(props: CardProps) {
  return (
    <div
      className={classNames(
        "rounded-md bg-white shadow-sm ring-1 ring-gray-900/5",
        props.className
      )}>
      {props.children}
    </div>
  );
}
