import { ReactNode } from "react";

type NtwDisplayProps = {
  children?: ReactNode;
  if: any;
};

export function NtwDisplay(props: NtwDisplayProps) {
  return <>{!!props.if ? props.children : null}</>;
}
