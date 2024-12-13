import { classNames } from "@navtrack/shared/utils/tailwind";
import { Icon } from "../icon/Icon";
import { faMaximize } from "@fortawesome/free-solid-svg-icons";
import { FormattedMessage } from "react-intl";
import { ZINDEX_MAP_CONTROL } from "../../../constants";
import { useMap } from "./useMap";
import { LatLongModel } from "@navtrack/shared/api/model/generated";
import { useEffect, useState } from "react";

type MapShowAllControlProps = {
  coordinates: LatLongModel[];
  options?: L.FitBoundsOptions;
};

export function MapShowAllControl(props: MapShowAllControlProps) {
  const map = useMap();

  const [initalized, setInitialized] = useState(false);

  useEffect(() => {
    if (
      props.coordinates !== undefined &&
      props.coordinates.length > 0 &&
      !initalized
    ) {
      map.fitBounds(props.coordinates, props.options);

      setInitialized(true);
    }
  }, [initalized, map, props.coordinates, props.options]);

  return (
    <div
      className="absolute bottom-0 mx-auto flex w-full justify-center"
      style={{ zIndex: ZINDEX_MAP_CONTROL }}>
      <div
        onClick={(e) => {
          map.fitBounds(props.coordinates, {
            paddingTopLeft: [60, 120],
            paddingBottomRight: [10, 10]
          });
          e.nativeEvent.stopImmediatePropagation();
          e.stopPropagation();
          e.preventDefault();
        }}
        className={classNames(
          "mb-2 cursor-pointer rounded-lg bg-white px-2 py-0.5 text-sm font-medium text-gray-900 shadow-md hover:bg-gray-100"
        )}>
        <Icon icon={faMaximize} className="mr-1" />
        <FormattedMessage id="generic.show-all" />
      </div>
    </div>
  );
}
