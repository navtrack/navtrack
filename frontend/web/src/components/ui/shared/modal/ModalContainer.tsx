import classNames from "classnames";
import { ReactNode } from "react";

interface IModalContainer {
  children?: ReactNode;
  className?: string;
}

export function ModalContainer(props: IModalContainer) {
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
