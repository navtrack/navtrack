import { classNames } from "@navtrack/shared/utils/tailwind";
import { Icon } from "../icon/Icon";
import { faLocationArrow } from "@fortawesome/free-solid-svg-icons";
import { FormattedMessage } from "react-intl";
import { useMap } from "./useMap";

export function MapShowAllControl() {
  const map = useMap();

  return (
    <div className="absolute bottom-0 z-10 mx-auto flex w-full justify-center">
      <div
        onClick={() => map.showAllMarkers()}
        className={classNames(
          "mb-2 cursor-pointer rounded-lg bg-white px-2 py-0.5 text-sm font-medium text-gray-900 shadow-md hover:bg-gray-100"
        )}>
        <Icon icon={faLocationArrow} className="mr-1" />
        <FormattedMessage id="generic.show-all" />
      </div>
    </div>
  );
}
