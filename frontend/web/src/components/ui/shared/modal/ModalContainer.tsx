import { classNames } from "@navtrack/shared/utils/tailwind";
import { ReactNode } from "react";

type ModalContainerProps = {
  children?: ReactNode;
  className?: string;
};

export function ModalContainer(props: ModalContainerProps) {
  return (
    <div
      className={classNames(
        "overflow-hidden rounded-md bg-white",
        props.className
      )}>
      {props.children}
    </div>
  );
}
