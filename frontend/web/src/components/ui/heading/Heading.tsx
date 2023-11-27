import { ReactNode } from "react";

type HeadingProps = {
  type: "h1" | "h2" | "h3";
  children: ReactNode;
};

export function Heading(props: HeadingProps) {
  switch (props.type) {
    case "h1":
      return (
        <h1 className="text-2xl font-bold leading-7 text-gray-900">
          {props.children}
        </h1>
      );
    case "h2":
      return (
        <h2 className="text-lg font-medium leading-6 text-gray-900">
          {props.children}
        </h2>
      );
    case "h3":
      return (
        <h3 className="text-base font-semibold leading-6 text-gray-900">
          {props.children}
        </h3>
      );
  }
}
