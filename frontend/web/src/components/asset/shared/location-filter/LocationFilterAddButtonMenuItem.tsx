import { FormattedMessage } from "react-intl";
import { IconWithText } from "../../../ui/icon/IconWithText";
import { IconProp } from "@fortawesome/fontawesome-svg-core";

type LocationFilterAddButtonMenuItemProps = {
  onClick: React.MouseEventHandler<HTMLSpanElement>;
  icon: IconProp;
  labelId: string;
};

export function LocationFilterAddButtonMenuItem(
  props: LocationFilterAddButtonMenuItemProps
) {
  return (
    <span
      className={
        "block cursor-pointer px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 hover:text-gray-900"
      }
      onClick={props.onClick}>
      <IconWithText icon={props.icon}>
        <FormattedMessage id={props.labelId} />
      </IconWithText>
    </span>
  );
}
