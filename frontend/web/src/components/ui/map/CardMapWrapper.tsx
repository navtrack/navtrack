import { ReactNode } from "react";

type CardMapWrapperProps = {
  children?: ReactNode;
  style?: React.CSSProperties;
};

export function CardMapWrapper(props: CardMapWrapperProps) {
  return (
    <div
      className="flex grow overflow-hidden rounded-md"
      style={props.style}>
      {props.children}
    </div>
  );
}
