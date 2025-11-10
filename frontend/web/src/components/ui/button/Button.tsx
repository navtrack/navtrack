import React, { ReactNode } from "react";
import { c, classNames } from "@navtrack/shared/utils/tailwind";
import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { Icon } from "../icon/Icon";
import { LoadingIndicator } from "@navtrack/shared/components/components/ui/loading-indicator/LoadingIndicator";

type ButtonProps = {
  children?: ReactNode;
  type?: "button" | "submit" | "reset";
  size?: "base" | "xs" | "sm" | "md" | "lg";
  color?: "primary" | "secondary" | "white" | "success" | "error";
  onClick?: React.MouseEventHandler<HTMLButtonElement>;
  isLoading?: boolean;
  disabled?: boolean;
  full?: boolean;
  icon?: IconProp;
  title?: string;
};

export function Button(props: ButtonProps) {
  return (
    <button
      type={props.type ?? "button"}
      onClick={props.onClick}
      title={props.title}
      disabled={props.isLoading || props.disabled}
      className={classNames(
        "whitespace-nowrap font-semibold shadow-sm hover:cursor-pointer",
        c(props.size === "xs", "h-6 rounded px-2 text-xs"),
        c(props.size === "sm", "h-7 rounded px-2.5 text-sm"),
        c(
          props.size === undefined || props.size === "base",
          "h-8 rounded px-3 text-sm"
        ),
        c(props.size === "md", "h-9 rounded px-3.5 text-sm"),
        c(props.size === "lg", "h-10 rounded px-4 text-sm"),
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
        {props.isLoading && (
          <LoadingIndicator
            className={props.children !== undefined ? "mr-2" : undefined}
          />
        )}
        {props.icon && !props.isLoading && (
          <Icon
            icon={props.icon}
            className={classNames(
              c(
                props.children !== undefined,
                c(props.size === "xs", "mr-1", "mr-2")
              ),
              c(props.size === "xs", "text-xs"),
              c(props.size === "sm", "text-xs"),
              c(props.size === undefined || props.size === "base", "text-sm"),
              c(props.size === "md", "text-base"),
              c(props.size === "lg", "text-lg")
            )}
          />
        )}
        {props.children}
      </div>
    </button>
  );
}
