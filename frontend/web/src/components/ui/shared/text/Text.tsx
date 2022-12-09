import { c } from "@navtrack/ui-shared/utils/tailwind";
import classNames from "classnames";
import { ReactNode } from "react";

export interface ITextProps {
  children: ReactNode;
  padding?: number;
  type?: "h3";
}

export default function Text(props: ITextProps) {
  return (
    <span
      className={classNames(
        c(props.type === "h3", "text-lg font-medium leading-6")
      )}>
      {props.children}
    </span>
  );
}
