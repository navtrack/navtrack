import React, { ReactNode } from "react";
import { LoadingIndicator } from "../loading-indicator/LoadingIndicator";
import { c, classNames } from "@navtrack/shared/utils/tailwind";

type ButtonProps = {
  children: ReactNode;
  size?: "base" | "xs" | "sm" | "md" | "lg";
  color?: "primary" | "secondary" | "white" | "warn" | "green";
  onClick?: React.MouseEventHandler<HTMLButtonElement>;
  className?: string;
  disabled?: boolean;
  type?: "button" | "submit" | "reset";
  fullWidth?: boolean;
  loading?: boolean;
};

export function Button(props: ButtonProps) {
  return (
    <button
      className={classNames(
        "inline-flex items-center justify-center rounded-md border font-medium shadow-sm focus:outline-none focus:ring-2 focus:ring-offset-2",
        c(props.size === "xs", "px-1.5 py-0.5 text-xs"),
        c(props.size === "sm", "px-2 py-1 text-xs"),
        c(props.size === "md", "px-3 py-1.5 text-xs"),
        c(
          props.size === "base" || props.size === undefined,
          "px-3.5 py-2 text-xs"
        ),
        c(props.size === "lg", "px-4 py-2 text-sm"),
        c(
          props.color === "primary" || props.color === undefined,
          "border-transparent bg-gray-700 text-white hover:bg-gray-800 focus:ring-gray-600"
        ),
        c(
          props.color === "secondary",
          "border-transparent bg-gray-100 text-gray-800 hover:bg-gray-200 focus:ring-gray-500"
        ),
        c(
          props.color === "white",
          "border-gray-300 bg-white text-gray-700 hover:bg-gray-50 focus:ring-gray-500"
        ),
        c(
          props.color === "green",
          "border-green-700 bg-green-600 text-white hover:border-green-800 hover:bg-green-700 focus:ring-green-500"
        ),
        c(
          props.color === "warn",
          "border-gray-300 bg-gray-50 text-red-700 hover:border-red-900 hover:bg-red-800 hover:text-white focus:ring-indigo-500"
        ),
        c(!!props.fullWidth, "w-full")
      )}
      onClick={props.onClick}
      disabled={props.disabled || props.loading}
      type={props.type ?? "button"}>
      {props.loading && <LoadingIndicator className="mr-2" />}
      {props.children}
    </button>
  );
}
