import classNames from "classnames";
import { ReactNode } from "react";

interface IModalBody {
  children?: ReactNode;
  className?: string;
}

export function ModalBody(props: IModalBody) {
  return (
    <div className={classNames("flex-grow p-4", props.className)}>
      {props.children}
    </div>
  );
}
