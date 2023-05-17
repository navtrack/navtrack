import { classNames } from "@navtrack/shared/utils/tailwind";
import { ReactNode } from "react";

type ModalBodyProps = {
  children?: ReactNode;
  className?: string;
};

export function ModalBody(props: ModalBodyProps) {
  return (
    <div className={classNames("flex-grow p-4", props.className)}>
      {props.children}
    </div>
  );
}
