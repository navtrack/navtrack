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
import {
  LocationFilterPage,
  useLocationFilterKey
} from "./useLocationFilterKey";

interface ILocationFilter {
  center?: LatLng;
  duration?: boolean;
  avgSpeed?: boolean;
  avgAltitude?: boolean;
  filterPage: LocationFilterPage;
}

export default function LocationFilter(props: ILocationFilter) {
  const filterKey = useLocationFilterKey(props.filterPage);

  return (
    <>
      <div className="flex">
        <Card className="flex h-10 flex-grow items-center space-x-2 p-2 text-xs text-gray-600">
          <Icon icon={faFilter} />
          <DateFilterBadge filterKey={filterKey} />
          <GeofenceFilterBadge filterKey={filterKey} />
          <SpeedFilterBadge filterKey={filterKey} />
          <AltitudeFilterBadge filterKey={filterKey} />
          {props.duration && <DurationFilterBadge filterKey={filterKey} />}
          <LocationFilterAddButton
            filterKey={filterKey}
            duration={props.duration}
            avgAltitude={props.avgAltitude}
            avgSpeed={props.avgSpeed}
          />
        </Card>
      </div>
      <DateFilterModal filterKey={filterKey} />
      <GeofenceFilterModal
        filterKey={filterKey}
        initialMapCenter={props.center}
      />
      <SpeedFilterModal filterKey={filterKey} average={props.avgSpeed} />
      <AltitudeFilterModal filterKey={filterKey} average={props.avgAltitude} />
      {props.duration && <DurationFilterModal filterKey={filterKey} />}
    </>
  );
}
