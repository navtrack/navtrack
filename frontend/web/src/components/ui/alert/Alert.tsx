import { c, classNames } from "@navtrack/shared/utils/tailwind";
import { ReactNode } from "react";
import { Icon } from "../icon/Icon";
import { faXmark } from "@fortawesome/free-solid-svg-icons";

type AlertProps = {
  className?: string;
  children?: ReactNode;
  close?: () => void;
  color?: "red" | "blue";
};

export function Alert(props: AlertProps) {
  return (
    <div
      className={classNames(
        "rounded-md px-4 py-3 text-sm",
        c(
          props.color === undefined || props.color === "red",
          "bg-red-100 text-red-700"
        ),
        c(props.color === "blue", "bg-blue-100 text-blue-700"),
        props.className
      )}>
      <div className="flex justify-between">
        <div className="flex items-center">{props.children}</div>
        {props.close !== undefined && (
          <div>
            <button
              type="button"
              onClick={props.close}
              className={classNames(
                "inline-flex rounded-md p-1.5 focus:outline-none focus:ring-2 focus:ring-offset-2",
                c(
                  props.color === undefined || props.color === "red",
                  "text-red-500 hover:bg-red-200 focus:ring-red-600 focus:ring-offset-red-100"
                ),
                c(
                  props.color === "blue",
                  "text-blue-500 hover:bg-blue-200 focus:ring-blue-600 focus:ring-offset-blue-100"
                )
              )}>
              <Icon icon={faXmark} className="h-5 w-5" />
            </button>
          </div>
        )}
      </div>
    </div>
  );
}
