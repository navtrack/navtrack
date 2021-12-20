import React, { ReactNode, Ref } from "react";
import classNames from "classnames";

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

const Button = React.forwardRef((props: Props, ref?: Ref<HTMLButtonElement>) => {
  return (
    <button
      ref={ref}
      disabled={props.disabled}
      className={classNames(
        {
          "opacity-75 cursor-auto": props.disabled,
          "text-xs px-4": props.size === "xs",
          "text-sm px-5": props.size === "sm" || props.size === undefined,
          "px-6 py-2": props.size === "lg",
          "bg-green-600 text-white border border-green-700": props.color === "primary",
          "hover:bg-green-700 hover:border-green-800": props.color === "primary" && !props.disabled,
          "bg-gray-700 text-white border border-gray-800": props.color === "secondary",
          "hover:bg-gray-800 hover:border-gray-900": props.color === "secondary" && !props.disabled,
          "bg-gray-100 text-gray-900 border border-gray-300": props.color === "basic",
          "hover:bg-gray-200 hover:border-gray-400": props.color === "basic" && !props.disabled,
          "bg-gray-100 text-red-700 border border-gray-300": props.color === "warn",
          "hover:bg-red-700 hover:text-white hover:border-red-700":
            props.color === "warn" && !props.disabled,
          "w-full": props.fullWidth
        },
        "py-1 rounded-lg focus:outline-none font-medium",
        props.className
      )}
      onClick={props.onClick}
      type={props.type}>
      {props.children}
    </button>
  );
});
