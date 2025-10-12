import { Link, generatePath } from "react-router-dom";
import { IconWithText } from "../../icon/IconWithText";
import { faCircle } from "@fortawesome/free-solid-svg-icons";
import { faCircle as faCircleRegular } from "@fortawesome/free-regular-svg-icons";
import { useScrollToAsset } from "./useScrollToAsset";
import { AssetModel } from "@navtrack/shared/api/model";
import { useCurrentAsset } from "@navtrack/shared/hooks/current/useCurrentAsset";
import { classNames } from "@navtrack/shared/utils/tailwind";
import { Paths } from "../../../../app/Paths";

type AuthenticatedLayoutSidebarItemProps = {
  asset: AssetModel;
};

export function AuthenticatedLayoutSidebarItem(
  props: AuthenticatedLayoutSidebarItemProps
) {
  const currentAsset = useCurrentAsset();
  const { elementRef } = useScrollToAsset(props.asset);

  return (
    <Link
      ref={elementRef}
      to={generatePath(Paths.AssetLive, { id: props.asset.id })}
      className={classNames(
        currentAsset.data === props.asset
          ? "bg-gray-900 text-white"
          : "text-gray-300 hover:bg-gray-700 hover:text-white",
        "flex cursor-pointer items-center rounded-md px-2 py-3 text-sm font-medium"
      )}>
      <IconWithText
        icon={props.asset.online ? faCircle : faCircleRegular}
        iconClassName={classNames(
          "text-xs",
          props.asset.online ? "text-green-400" : "text-red-600"
        )}>
        {props.asset.name}
      </IconWithText>
    </Link>
  );
}
