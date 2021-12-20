import { faFilter } from "@fortawesome/free-solid-svg-icons";
import DateFilterModal from "./date/DateFilterModal";
import SpeedFilterModal from "./speed/SpeedFilterModal";
import AltitudeFilterModal from "./altitude/AltitudeFilterModal";
import GeofenceFilterModal from "./geofence/GeofenceFilterModal";
import GeofenceFilterBadge from "./geofence/GeofenceFilterBadge";
import DateFilterBadge from "./date/DateFilterBadge";
import SpeedFilterBadge from "./speed/SpeedFilterBadge";
import AltitudeFilterBadge from "./altitude/AltitudeFilterBadge";
import LocationFilterAddButton from "./LocationFilterAddButton";
import Card from "../../../ui/shared/card/Card";
import { LatLng } from "../../../ui/shared/map/types";
import Icon from "../../../ui/shared/icon/Icon";
import DurationFilterBadge from "./duration/DurationFilterBadge";
import DurationFilterModal from "./duration/DurationFilterModal";
import useCurrentAssetId from "../../../../hooks/assets/useCurrentAssetId";
import { useMemo } from "react";

interface ILocationFilter {
  center?: LatLng;
  duration?: boolean;
  avgSpeed?: boolean;
  avgAltitude?: boolean;
  filterKey: string;
}

export default function LocationFilter(props: ILocationFilter) {
  const currentAssetId = useCurrentAssetId();
  const key = useMemo(
    () => `${props.filterKey}:${currentAssetId}`,
    [currentAssetId, props.filterKey]
  );

  return (
    <>
      <div className="flex">
        <Card className="flex flex-grow text-xs p-2 items-center text-gray-600 space-x-2 h-10">
          <Icon icon={faFilter} />
          <DateFilterBadge filterKey={key} />
          <GeofenceFilterBadge filterKey={key} />
          <SpeedFilterBadge filterKey={key} />
          <AltitudeFilterBadge filterKey={key} />
          {props.duration && <DurationFilterBadge filterKey={key} />}
          <LocationFilterAddButton
            filterKey={key}
            duration={props.duration}
            avgAltitude={props.avgAltitude}
            avgSpeed={props.avgSpeed}
          />
        </Card>
      </div>
      <DateFilterModal filterKey={key} />
      <GeofenceFilterModal filterKey={key} initialMapCenter={props.center} />
      <SpeedFilterModal filterKey={key} average={props.avgSpeed} />
      <AltitudeFilterModal filterKey={key} average={props.avgAltitude} />
      {props.duration && <DurationFilterModal filterKey={key} />}
    </>
  );
}
