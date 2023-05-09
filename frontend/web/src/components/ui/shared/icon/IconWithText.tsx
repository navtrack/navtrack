import { IconProp, SizeProp } from "@fortawesome/fontawesome-svg-core";
import classNames from "classnames";
import { ReactNode } from "react";
import { Icon } from "./Icon";

interface IIconsWithText {
  icon: IconProp;
  spin?: boolean;
  hidden?: boolean;
  size?: SizeProp;
  children: ReactNode;
  iconClassName?: string;
  className?: string;
}

export function IconWithText(props: IIconsWithText) {
  return (
    <div className={classNames(props.className, "flex")}>
      <div
        className={classNames(
          "mr-2 flex w-4 items-center",
          props.iconClassName
        )}>
        <Icon icon={props.icon} size={props.size} spin={props.spin} />
      </div>
      <div>{props.children}</div>
    </div>
  );
}
