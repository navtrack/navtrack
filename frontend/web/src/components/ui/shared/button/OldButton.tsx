import { c, classNames } from "@navtrack/shared/utils/tailwind";
import React, { ReactNode, Ref } from "react";

type Props = {
  children: ReactNode;
  size?: "sm" | "xs" | "lg";
  color: "primary" | "secondary" | "basic" | "warn";
  onClick?: () => void;
  className?: string;
  disabled?: boolean;
  type?: "button" | "submit" | "reset";
  fullWidth?: boolean;
};

const Button = React.forwardRef(
  (props: Props, ref?: Ref<HTMLButtonElement>) => {
    return (
      <button
        ref={ref}
        disabled={props.disabled}
        className={classNames(
          c(props.disabled, "cursor-auto opacity-75"),
          c(props.size === "xs", "px-4 text-xs"),
          c(props.size === "sm" || props.size === undefined, "px-5 text-sm"),
          c(props.size === "lg", "px-6 py-2"),
          c(
            props.color === "primary",
            "border border-green-700 bg-green-600 text-white"
          ),
          c(
            props.color === "primary" && !props.disabled,
            "hover:border-green-800 hover:bg-green-700"
          ),
          c(
            props.color === "secondary",
            "border border-gray-800 bg-gray-700 text-white"
          ),
          c(
            props.color === "secondary" && !props.disabled,
            "hover:border-gray-900 hover:bg-gray-800"
          ),
          c(
            props.color === "basic",
            "border border-gray-300 bg-gray-100 text-gray-900"
          ),
          c(
            props.color === "basic" && !props.disabled,
            "hover:border-gray-400 hover:bg-gray-200"
          ),
          c(
            props.color === "warn",
            "border border-gray-300 bg-gray-100 text-red-700"
          ),
          c(
            props.color === "warn" && !props.disabled,
            "hover:border-red-700 hover:bg-red-700 hover:text-white"
          ),
          c(props.fullWidth, "w-full"),
          "rounded-lg py-1 font-medium focus:outline-none",
          props.className
        )}
        onClick={props.onClick}
        type={props.type}>
        {props.children}
      </button>
    );
  }
);
