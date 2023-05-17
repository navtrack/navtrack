import { useHistory } from "react-router-dom";
import { IconWithText } from "../../shared/icon/IconWithText";
import { faCircle } from "@fortawesome/free-solid-svg-icons";
import { faCircle as faCircleRegular } from "@fortawesome/free-regular-svg-icons";
import { useScrollToAsset } from "./useScrollToAsset";
import { AssetModel } from "@navtrack/shared/api/model/generated";
import { useCurrentAsset } from "@navtrack/shared/hooks/assets/useCurrentAsset";
import { classNames } from "@navtrack/shared/utils/tailwind";

export type AdminLayoutSideBarItemProps = {
  asset: AssetModel;
};

export function AdminLayoutSideBarItem(props: AdminLayoutSideBarItemProps) {
  const currentAsset = useCurrentAsset();
  const history = useHistory();
  const { elementRef } = useScrollToAsset(props.asset);

  return (
    <a
      ref={elementRef}
      href={`/assets/${props.asset.id}/live`}
      className={classNames(
        currentAsset.data === props.asset
          ? "bg-gray-900 text-white"
          : "text-gray-300 hover:bg-gray-700 hover:text-white",
        "flex cursor-pointer items-center rounded-md px-2 py-3 text-sm font-medium"
      )}
      onClick={(e) => {
        e.preventDefault();
        history.push(`/assets/${props.asset.id}/live`);
      }}>
      <IconWithText
        icon={props.asset.online ? faCircle : faCircleRegular}
        iconClassName={classNames(
          "text-xs",
          props.asset.online ? "text-green-400" : "text-red-600"
        )}>
        {props.asset.name}
      </IconWithText>
    </a>
  );
}
