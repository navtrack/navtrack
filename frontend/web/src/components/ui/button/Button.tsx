import React, { ReactNode } from "react";
import { c, classNames } from "@navtrack/shared/utils/tailwind";
import { NtwLoadingIndicator } from "../loading-indicator/NtwLoadingIndicator";
import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { Icon } from "../icon/Icon";

type ButtonProps = {
  children?: ReactNode;
  type?: "button" | "submit" | "reset";
  size?: "base" | "xs" | "sm" | "md" | "lg";
  color?: "primary" | "secondary" | "white" | "success" | "error";
  onClick?: React.MouseEventHandler<HTMLButtonElement>;
  loading?: boolean;
  disabled?: boolean;
  full?: boolean;
  icon?: IconProp;
};

export function Button(props: ButtonProps) {
  return (
    <button
      type={props.type}
      onClick={props.onClick}
      disabled={props.loading || props.disabled}
      className={classNames(
        "font-semibold shadow-sm",
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
          "bg-blue-700 text-white hover:bg-blue-800 disabled:bg-blue-500 disabled:opacity-90"
        ),
        c(
          props.color === "secondary",
          "bg-gray-700 text-white hover:bg-gray-800 disabled:bg-gray-500 disabled:opacity-90"
        ),
        c(
          props.color === "white",
          "bg-gray-50 text-gray-900 ring-1 ring-inset ring-gray-300 hover:bg-gray-200 disabled:bg-gray-50 disabled:opacity-60"
        ),
        c(
          props.color === "success",
          "bg-green-600 text-white hover:border-green-700 hover:bg-green-700 disabled:bg-green-600 disabled:opacity-90"
        ),
        c(
          props.color === "error",
          "bg-gray-50 text-red-700 ring-1 ring-inset ring-gray-300 hover:bg-red-700 hover:text-white hover:ring-red-700 disabled:bg-gray-50 disabled:text-red-700 disabled:opacity-60 disabled:ring-gray-300"
        ),
        c(props.full, "w-full")
      )}>
      <div className="flex items-center justify-center">
        {props.loading && (
          <NtwLoadingIndicator
            className={props.children !== undefined ? "mr-2" : undefined}
          />
        )}
        {props.icon && !props.loading && (
          <Icon
            icon={props.icon}
            className={props.children !== undefined ? "mr-2" : undefined}
          />
        )}
        {props.children}
      </div>
    </button>
  );
}
