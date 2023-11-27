import { classNames } from "@navtrack/shared/utils/tailwind";
import { ReactNode } from "react";

type CardFooterProps = {
  children?: ReactNode;
  className?: string;
};

export function CardFooter(props: CardFooterProps) {
  return (
    <div
      className={classNames(
        "border-t border-gray-900/10 px-6 py-4",
        props.className
      )}>
      {props.children}
    </div>
  );
}
