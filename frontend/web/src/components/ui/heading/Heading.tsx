import { classNames } from "@navtrack/shared/utils/tailwind";
import { ReactNode } from "react";

type HeadingProps = {
  type: "h1" | "h2" | "h3";
  children: ReactNode;
  className?: string;
};

export function Heading(props: HeadingProps) {
  switch (props.type) {
    case "h1":
      return (
        <h1
          className={classNames(
            "text-2xl font-bold leading-7 text-gray-900",
            props.className
          )}>
          {props.children}
        </h1>
      );
    case "h2":
      return (
        <h2
          className={classNames(
            "text-lg font-medium leading-6 text-gray-900",
            props.className
          )}>
          {props.children}
        </h2>
      );
    case "h3":
      return (
        <h3
          className={classNames(
            "text-base font-semibold leading-6 text-gray-900",
            props.className
          )}>
          {props.children}
        </h3>
      );
  }
}
