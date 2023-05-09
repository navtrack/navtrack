import { ReactNode } from "react";

interface IModalContent {
  children?: ReactNode;
}

export function ModalContent(props: IModalContent) {
  return <div className="flex">{props.children}</div>;
}
