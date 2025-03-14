import { classNames } from "@navtrack/shared/utils/tailwind";
import { Icon } from "../icon/Icon";
import { faMaximize } from "@fortawesome/free-solid-svg-icons";
import { FormattedMessage } from "react-intl";
import { ZINDEX_MAP_CONTROL } from "../../../constants";
import { useMap } from "./useMap";
import { LatLong } from "@navtrack/shared/api/model/generated";
import { useEffect, useState } from "react";
import { MapOptionsDto } from "@navtrack/shared/maps";

type MapShowAllControlProps = {
  coordinates: LatLong[];
  options?: MapOptionsDto;
};

export function MapShowAllControl(props: MapShowAllControlProps) {
  const map = useMap();

  const [initialized, setInitialized] = useState(false);

  useEffect(() => {
    if (
      props.coordinates !== undefined &&
      props.coordinates.length > 0 &&
      !initialized
    ) {
      map.fitBounds(props.coordinates, props.options);

      setInitialized(true);
    }
  }, [initialized, map, props.coordinates, props.options]);

  return (
    <div
      className="absolute bottom-0 mx-auto flex w-full justify-center"
      style={{ zIndex: ZINDEX_MAP_CONTROL }}>
      <div
        onClick={(e) => {
          map.fitBounds(props.coordinates, props.options);
          e.stopPropagation();
        }}
        onDoubleClickCapture={(e) => e.stopPropagation()}
        onDoubleClick={(e) => e.nativeEvent.stopPropagation()}
        className={classNames(
          "mb-2 cursor-pointer rounded-lg bg-white px-2 py-0.5 text-sm font-medium text-gray-900 shadow-md hover:bg-gray-100"
        )}>
        <Icon icon={faMaximize} className="mr-1" />
        <FormattedMessage id="generic.show-all" />
      </div>
    </div>
  );
}
