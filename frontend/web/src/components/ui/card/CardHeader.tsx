import { classNames } from "@navtrack/shared/utils/tailwind";
import { ReactNode } from "react";

type CardHeaderProps = {
  children?: ReactNode;
  className?: string;
};

export function CardHeader(props: CardHeaderProps) {
  return (
    <div className={classNames("px-6 pt-6", props.className)}>
      {props.children}
    </div>
  );
}
