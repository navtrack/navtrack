import React, { ReactNode } from "react";
import classNames from "classnames";

type Props = {
  children: ReactNode,
  size?: "sm",
  color: "primary" | "secondary"
  onClick: () => void,
  className?: string
}

export default function Button(props: Props) {
  return (
    <button className={classNames("shadow  py-1 px-4 rounded focus:outline-none text-sm", props.className,
      {
        "bg-gray-800 hover:bg-gray-700 text-white font-medium": props.color === "primary",
        "bg-gray-300 hover:bg-gray-200 text-gray-700": props.color === "secondary",
        "a": props.size === "sm"
      })}
      onClick={props.onClick} >
      {props.children}
    </button >
  );;
}