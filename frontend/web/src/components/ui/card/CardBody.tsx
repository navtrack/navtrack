import { classNames } from "@navtrack/shared/utils/tailwind";
import { ReactNode } from "react";

export type CardBodyProps = {
  children?: ReactNode;
  className?: string;
};

export function CardBody(props: CardBodyProps) {
  return (
    <div className={classNames("p-6", props.className)}>{props.children}</div>
  );
}
