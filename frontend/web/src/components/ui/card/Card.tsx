import { classNames } from "@navtrack/shared/utils/tailwind";
import { ReactNode } from "react";

export type CardProps = {
  children?: ReactNode;
  className?: string;
  ref?: React.Ref<HTMLDivElement>;
  style?: React.CSSProperties;
};

export function Card(props: CardProps) {
  return (
    <div
      style={props.style}
      ref={props.ref}
      className={classNames(
        "rounded-md bg-white shadow-sm ring-1 ring-gray-900/5",
        props.className
      )}>
      {props.children}
    </div>
  );
}
