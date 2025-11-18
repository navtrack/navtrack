import { ReactNode } from "react";
import { Icon } from "../icon/Icon";
import { faCircle } from "@fortawesome/free-solid-svg-icons";
import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { c, classNames } from "@navtrack/shared/utils/tailwind";

type TimelineItemProps = {
  title: ReactNode;
  subTitle?: ReactNode;
  icon?: IconProp;
  last?: boolean;
};

export function TimelineItem(props: TimelineItemProps) {
  return (
    <div>
      <div className="flex">
        <div className="w-4 flex items-center justify-center">
          <Icon icon={props.icon ?? faCircle} size="xs" />
        </div>
        <div className="flex items-center text-sm ml-2 font-medium">
          {props.title}
        </div>
      </div>
      <div className="flex">
        <div className="w-4 flex items-center justify-center">
          {!props.last && <div className="w-px bg-gray-900 h-full" />}
        </div>
        <div
          className={classNames(
            "flex items-center flex-col text-xs ml-2",
            c(!props.last, "pb-4")
          )}>
          {props.subTitle}
        </div>
      </div>
    </div>
  );
}
