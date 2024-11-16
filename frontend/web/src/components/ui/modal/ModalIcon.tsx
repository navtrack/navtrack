import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { Icon } from "../icon/Icon";

type ModalIconProps = {
  icon: IconProp;
};

export function ModalIcon(props: ModalIconProps) {
  return (
    <div className="py-6 pl-6">
      <div className="flex h-10 w-10 items-center justify-center rounded-full bg-gray-900 text-white">
        <Icon icon={props.icon} size="sm" />
      </div>
    </div>
  );
}
