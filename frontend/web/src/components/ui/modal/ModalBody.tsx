import { classNames } from "@navtrack/shared/utils/tailwind";
import { ReactNode } from "react";

type ModalBodyProps = {
  children?: ReactNode;
  className?: string;
};

export function ModalBody(props: ModalBodyProps) {
  return (
    <div className={classNames("grow p-6", props.className)}>
      {props.children}
    </div>
  );
}
