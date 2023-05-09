import React from "react";
import { Icon } from "../icon/Icon";
import { IconProp } from "@fortawesome/fontawesome-svg-core";

interface IIconButton {
  onClick?: React.MouseEventHandler<HTMLButtonElement>;
  className?: string;
  disabled?: boolean;
  icon: IconProp;
}

export function IconButton(props: IIconButton) {
  return (
    <button onClick={props.onClick} disabled={props.disabled}>
      <Icon icon={props.icon} className={props.className} />
    </button>
  );
}
