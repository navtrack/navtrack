import { IconProp, SizeProp } from "@fortawesome/fontawesome-svg-core";
import { ReactNode } from "react";
import { Icon } from "./Icon";
import { classNames } from "@navtrack/shared/utils/tailwind";

type IconWithTextProps = {
  icon: IconProp;
  spin?: boolean;
  hidden?: boolean;
  size?: SizeProp;
  children: ReactNode;
  iconClassName?: string;
  className?: string;
};

export function IconWithText(props: IconWithTextProps) {
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
