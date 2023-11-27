import { c, classNames } from "@navtrack/shared/utils/tailwind";
import { ReactNode } from "react";

export type TextProps = {
  children: ReactNode;
  padding?: number;
  type?: "h3";
};

export function Text(props: TextProps) {
  return (
    <span
      className={classNames(
        c(props.type === "h3", "text-lg font-medium leading-6")
      )}>
      {props.children}
    </span>
  );
}
