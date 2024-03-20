import { ReactNode } from "react";

type ModalContentProps = {
  children?: ReactNode;
};

export function ModalContent(props: ModalContentProps) {
  return <div className="flex">{props.children}</div>;
}
