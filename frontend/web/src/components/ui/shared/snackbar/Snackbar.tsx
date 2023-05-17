import {
  faCheckCircle,
  faExclamationCircle,
  faInfoCircle,
  faTimes
} from "@fortawesome/free-solid-svg-icons";
import { Transition } from "@headlessui/react";
import { c, classNames } from "@navtrack/shared/utils/tailwind";
import { Fragment, useMemo } from "react";
import { Icon } from "../icon/Icon";

export type SnackbarType = "success" | "error" | "info";

type SnackbarProps = {
  type?: SnackbarType;
  title?: string;
  description?: string;
  show: boolean;
  onCloseClick?: () => void;
};

export function Snackbar(props: SnackbarProps) {
  const icon = useMemo(() => {
    if (props.type === "error") {
      return faExclamationCircle;
    }
    if (props.type === "info") {
      return faInfoCircle;
    }

    return faCheckCircle;
  }, [props.type]);

  return (
    <Transition
      as={Fragment}
      show={props.show}
      enter="transition duration-300 ease-out"
      enterFrom="transform scale-95 opacity-0"
      enterTo="transform scale-100 opacity-100"
      leave="transition duration-100 ease-out"
      leaveFrom="transform scale-100 opacity-100"
      leaveTo="transform scale-95 opacity-0">
      <div
        className="absolute bottom-8 left-1/2 right-auto flex max-w-md rounded-lg border bg-white p-5 shadow-md"
        style={{ transform: "translateX(-50%)" }}>
        <div
          className={classNames("mr-5", c(!props.title, "flex items-center"))}>
          <Icon
            icon={icon}
            className={classNames(
              "text-2xl",
              c(
                props.type === undefined || props.type === "success",
                "text-green-600"
              ),
              c(props.type === "error", "text-red-500")
            )}
          />
        </div>
        <div className={classNames(c(!props.title, "flex items-center"))}>
          {props.title && (
            <div className="mb-2 font-semibold">{props.title}</div>
          )}
          <div className="text-sm text-gray-500">{props.description}</div>
        </div>
        <div
          className={classNames("ml-5", c(!props.title, "flex items-center"))}>
          <Icon
            icon={faTimes}
            size="lg"
            className="cursor-pointer hover:text-gray-600"
            onClick={props.onCloseClick}
          />
        </div>
      </div>
    </Transition>
  );
}
