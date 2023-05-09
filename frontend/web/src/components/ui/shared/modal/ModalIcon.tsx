import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { ReactNode } from "react";
import { Icon } from "../icon/Icon";

interface IModalIcon {
  children?: ReactNode;
  icon: IconProp;
}

export function ModalIcon(props: IModalIcon) {
  return (
    <div className="py-4 pl-4">
      <div className="mx-auto flex h-12 w-12 items-center justify-center rounded-full bg-gray-900 text-white  sm:mx-0 sm:h-10 sm:w-10">
        <Icon icon={props.icon} />
      </div>
    </div>
  );
}
