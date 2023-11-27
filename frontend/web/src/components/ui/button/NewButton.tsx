import React, { ReactNode } from "react";
import { c, classNames } from "@navtrack/shared/utils/tailwind";
import { NtwLoadingIndicator } from "../loading-indicator/NtwLoadingIndicator";

type ButtonProps = {
  children: ReactNode;
  type?: "button" | "submit" | "reset";
  size?: "base" | "xs" | "sm" | "md" | "lg";
  color?: "primary" | "secondary" | "warn";
  onClick?: React.MouseEventHandler<HTMLButtonElement>;
  loading?: boolean;
  disabled?: boolean;
  full?: boolean;
};

export function NewButton(props: ButtonProps) {
  return (
    <button
      type={props.type}
      onClick={props.onClick}
      disabled={props.loading || props.disabled}
      className={classNames(
        "disabled:opacity-60",
        c(
          props.size === undefined || props.size === "base",
          "rounded-md px-2.5 py-1.5 text-sm"
        ),
        c(props.size === "xs", "rounded px-2 py-1 text-xs"),
        c(props.size === "sm", "rounded px-2 py-1 text-sm"),
        c(props.size === "md", "rounded-md px-3 py-2 text-sm"),
        c(props.size === "lg", "rounded-md px-3.5 py-2.5 text-sm"),
        c(
          props.color === undefined || props.color === "primary",
          "bg-indigo-600 font-semibold text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600 disabled:bg-indigo-500"
        ),

        c(
          props.color === "secondary",
          "bg-gray-50 font-semibold text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 hover:bg-gray-100 disabled:bg-gray-50"
        ),
        c(
          props.color === "warn",
          "bg-gray-50 font-semibold text-red-600 shadow-sm ring-1 ring-inset ring-gray-300 hover:bg-gray-100 disabled:bg-gray-50"
        ),
        c(props.full, "w-full")
      )}>
      <div className="flex items-center justify-center">
        {props.loading && <NtwLoadingIndicator className="mr-2" />}
        {props.children}
      </div>
    </button>
  );
}
