import { classNames } from "@navtrack/shared/utils/tailwind";
import { ReactNode } from "react";

export type CardProps = {
  children: ReactNode;
  className?: string;
};

export function Card(props: CardProps) {
  return (
    <div className={classNames("rounded-lg bg-white shadow", props.className)}>
      {props.children}
    </div>
  );
}
