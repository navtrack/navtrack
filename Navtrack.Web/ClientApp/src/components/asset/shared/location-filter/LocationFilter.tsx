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
import { useMemo } from "react";
import { useCurrentAsset } from "@navtrack/navtrack-app-shared";

interface ILocationFilter {
  center?: LatLng;
  duration?: boolean;
  avgSpeed?: boolean;
  avgAltitude?: boolean;
  filterKey: string;
}

export default function LocationFilter(props: ILocationFilter) {
  const currentAsset = useCurrentAsset();
  const key = useMemo(
    () => `${props.filterKey}:${currentAsset?.id}`,
    [currentAsset, props.filterKey]
  );

  return (
    <>
      <div className="flex">
        <Card className="flex h-10 flex-grow items-center space-x-2 p-2 text-xs text-gray-600">
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
