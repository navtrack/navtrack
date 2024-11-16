import { classNames } from "@navtrack/shared/utils/tailwind";
import { ReactNode } from "react";

type BadgeFlatPillProps = {
  children?: ReactNode;
  className?: string;
  color?:
    | "darkGray"
    | "gray"
    | "red"
    | "yellow"
    | "green"
    | "blue"
    | "indigo"
    | "purple"
    | "pink";
};

export function BadgeFlatPill(props: BadgeFlatPillProps) {
  switch (props.color) {
    case "red":
      return (
        <span
          className={classNames(
            "inline-flex items-center rounded-full bg-red-100 px-2 py-1 text-xs font-medium text-red-700",
            props.className
          )}>
          {props.children}
        </span>
      );
    case "yellow":
      return (
        <span
          className={classNames(
            "inline-flex items-center rounded-full bg-yellow-100 px-2 py-1 text-xs font-medium text-yellow-800",
            props.className
          )}>
          {props.children}
        </span>
      );
    case "green":
      return (
        <span
          className={classNames(
            "inline-flex items-center rounded-full bg-green-100 px-2 py-1 text-xs font-medium text-green-700",
            props.className
          )}>
          {props.children}
        </span>
      );
    case "blue":
      return (
        <span
          className={classNames(
            "inline-flex items-center rounded-full bg-blue-100 px-2 py-1 text-xs font-medium text-blue-700",
            props.className
          )}>
          {props.children}
        </span>
      );
    case "indigo":
      return (
        <span
          className={classNames(
            "inline-flex items-center rounded-full bg-indigo-100 px-2 py-1 text-xs font-medium text-indigo-700",
            props.className
          )}>
          {props.children}
        </span>
      );
    case "purple":
      return (
        <span
          className={classNames(
            "inline-flex items-center rounded-full bg-purple-100 px-2 py-1 text-xs font-medium text-purple-700",
            props.className
          )}>
          {props.children}
        </span>
      );
    case "pink":
      return (
        <span
          className={classNames(
            "inline-flex items-center rounded-full bg-pink-100 px-2 py-1 text-xs font-medium text-pink-700",
            props.className
          )}>
          {props.children}
        </span>
      );
    case "darkGray":
      return (
        <span
          className={classNames(
            "inline-flex items-center rounded-full bg-gray-200 px-2 py-1 text-xs font-medium text-gray-600",
            props.className
          )}>
          {props.children}
        </span>
      );
    default:
      return (
        <span
          className={classNames(
            "inline-flex items-center rounded-full bg-gray-100 px-2 py-1 text-xs font-medium text-gray-600",
            props.className
          )}>
          {props.children}
        </span>
      );
  }
}
