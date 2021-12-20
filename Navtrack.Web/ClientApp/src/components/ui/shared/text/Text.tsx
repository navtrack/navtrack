import classNames from "classnames";
import { ReactNode } from "react";
import c from "../../../../utils/tailwind";

export interface ITextProps {
  children: ReactNode;
  padding?: number;
  type?: "h3";
}

export default function Text(props: ITextProps) {
  return (
    <span
      className={classNames(
        c(props.type === "h3", "text-lg leading-6 font-medium")
      )}>
      {props.children}
    </span>
  );
}
