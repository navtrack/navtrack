import React, { ReactNode } from "react";
import Label from "./Label";
import classNames from "classnames";

type Props = {
  children: ReactNode;
  readOnly?: boolean;
  checked: boolean;
  className?: string;
  onClick?: () => void;
};

export default function Checkbox(props: Props) {
  return (
    <div
      className={classNames("flex flex-row items-center", props.className)}
      onClick={props.onClick}>
      <input type="checkbox" checked={props.checked} readOnly={props.readOnly} />
      <Label className="ml-2">{props.children}</Label>
    </div>
  );
}
