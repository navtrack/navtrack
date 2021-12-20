import classNames from "classnames";
import { ReactNode } from "react";

interface IModalBody {
  children?: ReactNode;
  className?: string;
}

export default function ModalBody(props: IModalBody) {
  return <div className={classNames("p-4 flex-grow", props.className)}>{props.children}</div>;
}
