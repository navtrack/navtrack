import { NtwIcon } from "../icon/NtwIcon";
import { ReactNode } from "react";
import { faCheckCircle } from "@fortawesome/free-regular-svg-icons";
import { faXmark } from "@fortawesome/free-solid-svg-icons";
import { c, classNames } from "@navtrack/shared/utils/tailwind";

type NtwNotificationProps = {
  children?: ReactNode;
  type: "success" | "error";
  onDismiss?: () => void;
};

export function NtwNotification(props: NtwNotificationProps) {
  return (
    <div
      className={classNames(
        c(props.type === "success", "bg-green-50"),
        c(props.type === "error", "bg-red-50"),
        "rounded-md p-4"
      )}>
      <div className="flex">
        <div className="flex shrink-0 items-center">
          <NtwIcon
            icon={faCheckCircle}
            className={classNames(
              c(props.type === "success", "text-green-400"),
              c(props.type === "error", "text-red-400"),
              "h-5 w-5"
            )}
          />
        </div>
        <div className="ml-3 flex items-center">
          <p
            className={classNames(
              c(props.type === "success", "text-green-800"),
              c(props.type === "error", "text-red-800"),
              "text-sm font-medium"
            )}>
            {props.children}
          </p>
        </div>
        <div className="ml-auto pl-3">
          <div className="-mx-1.5 -my-1.5 flex items-center">
            <button
              onClick={props.onDismiss}
              type="button"
              className={classNames(
                c(
                  props.type === "success",
                  "bg-green-50 text-green-500 hover:bg-green-100 focus:ring-green-600 focus:ring-offset-green-50"
                ),
                c(
                  props.type === "error",
                  "bg-red-50 text-red-500 hover:bg-red-100 focus:ring-red-600 focus:ring-offset-red-50"
                ),
                "inline-flex rounded-md p-1.5 focus:outline-none focus:ring-2 focus:ring-offset-2"
              )}>
              <span className="sr-only">Dismiss</span>
              <NtwIcon
                icon={faXmark}
                className={classNames(
                  c(props.type === "success", "text-green-400"),
                  c(props.type === "error", "text-red-400"),
                  "h-5 w-5"
                )}
              />
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}
