import classNames from "classnames";
import { ReactNode } from "react";

interface IModalContainer {
  children?: ReactNode;
  className?: string;
}

export default function ModalContainer(props: IModalContainer) {
  return (
    <div className={classNames("bg-white rounded-md overflow-hidden", props.className)}>
      {props.children}
    </div>
  );
}
