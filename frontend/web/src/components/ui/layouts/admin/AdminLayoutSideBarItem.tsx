import classNames from "classnames";
import { useHistory } from "react-router";
import IconWithText from "../../shared/icon/IconWithText";
import { faCircle } from "@fortawesome/free-solid-svg-icons";
import { faCircle as faCircleRegular } from "@fortawesome/free-regular-svg-icons";
import useScrollToAsset from "./useScrollToAsset";
import { AssetModel } from "@navtrack/ui-shared/api/model/generated";
import { useCurrentAsset } from "@navtrack/ui-shared/newHooks/assets/useCurrentAsset";

export interface IAdminLayoutSideBarItemProps {
  asset: AssetModel;
}

export default function AdminLayoutSideBarItem(
  props: IAdminLayoutSideBarItemProps
) {
  const currentAsset = useCurrentAsset();
  const history = useHistory();
  const { elementRef } = useScrollToAsset(props.asset);

  return (
    <a
      ref={elementRef}
      href={`/assets/${props.asset.shortId}/live`}
      className={classNames(
        currentAsset === props.asset
          ? "bg-gray-900 text-white"
          : "text-gray-300 hover:bg-gray-700 hover:text-white",
        "flex cursor-pointer items-center rounded-md px-2 py-3 text-sm font-medium"
      )}
      onClick={(e) => {
        e.preventDefault();
        history.push(`/assets/${props.asset.shortId}/live`);
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
