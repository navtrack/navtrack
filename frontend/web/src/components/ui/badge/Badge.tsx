import { c, classNames } from "@navtrack/shared/utils/tailwind";
import { ReactNode } from "react";

export enum BadgeColor {
  Gray,
  Red,
  Yellow,
  Green,
  Indigo,
  Blue
}

export type BadgeProps = {
  children?: ReactNode;
  color?: BadgeColor;
  className?: string;
  size?: "sm" | "lg";
};

function getColor(color?: BadgeColor) {
  switch (color) {
    case BadgeColor.Red:
      return "bg-red-50 text-red-700 ring-red-600/10";
    case BadgeColor.Yellow:
      return "bg-yellow-50 text-yellow-800 ring-yellow-600/20";
    case BadgeColor.Green:
      return "bg-green-50 text-green-700 ring-green-600/20";
    case BadgeColor.Indigo:
      return "bg-indigo-50 text-indigo-700 ring-indigo-700/10";
    case BadgeColor.Blue:
      return "bg-blue-50 text-blue-700 ring-blue-700/10";
    default:
      return "bg-gray-50 text-gray-600 ring-gray-500/10";
  }
}

export function Badge(props: BadgeProps) {
  return (
    <span
      className={classNames(
        "inline-flex items-center rounded-md  font-medium ring-1 ring-inset",
        c(props.size === "sm", "px-2 py-0.5 text-xs"),
        c(props.size === "lg", "px-3 py-1 text-sm"),
        c(props.size === undefined, "px-2 py-1 text-xs"),
        getColor(props.color),
        props.className
      )}>
      {props.children}
    </span>
  );
}
