import { faFilter } from "@fortawesome/free-solid-svg-icons";
import { DateFilterModal } from "./date/DateFilterModal";
import { GeofenceFilterBadge } from "./geofence/GeofenceFilterBadge";
import { DateFilterBadge } from "./date/DateFilterBadge";
import {
  AverageSpeedFilterBadge,
  SpeedFilterBadge
} from "./speed/SpeedFilterBadge";
import { AltitudeFilterBadge } from "./altitude/AltitudeFilterBadge";
import { LocationFilterAddButton } from "./LocationFilterAddButton";
import { Card } from "../../../ui/card/Card";
import { Icon } from "../../../ui/icon/Icon";
import { DurationFilterBadge } from "./duration/DurationFilterBadge";
import { DurationFilterModal } from "./duration/DurationFilterModal";
import {
  LocationFilterConfiguration,
  LocationFilterType
} from "./locationFilterTypes";
import { GeofenceFilterModal } from "./geofence/GeofenceFilterModal";
import {
  AverageSpeedFilterModal,
  SpeedFilterModal
} from "./speed/SpeedFilterModal";
import { AltitudeFilterModal } from "./altitude/AltitudeFilterModal";

type LocationFilterProps = {
  configuration: LocationFilterConfiguration;
};

export function LocationFilter(props: LocationFilterProps) {
  const hasFilter = (filter: LocationFilterType) =>
    props.configuration.filters.includes(filter);

  return (
    <>
      <div className="flex">
        <Card className="flex h-10 grow items-center space-x-2 p-2 text-xs text-gray-600">
          <Icon icon={faFilter} />
          <DateFilterBadge filterKey={props.configuration.filterKey} />
          {hasFilter(LocationFilterType.Geofence) && (
            <GeofenceFilterBadge filterKey={props.configuration.filterKey} />
          )}
          {hasFilter(LocationFilterType.Speed) && (
            <SpeedFilterBadge filterKey={props.configuration.filterKey} />
          )}
          {hasFilter(LocationFilterType.AvgSpeed) && (
            <AverageSpeedFilterBadge
              filterKey={props.configuration.filterKey}
            />
          )}
          {hasFilter(LocationFilterType.Altitude) && (
            <AltitudeFilterBadge filterKey={props.configuration.filterKey} />
          )}
          {hasFilter(LocationFilterType.Duration) && (
            <DurationFilterBadge filterKey={props.configuration.filterKey} />
          )}
          <LocationFilterAddButton configuration={props.configuration} />
        </Card>
      </div>
      <DateFilterModal filterKey={props.configuration.filterKey} />
      {hasFilter(LocationFilterType.Geofence) && (
        <GeofenceFilterModal
          filterKey={props.configuration.filterKey}
          // initialMapCenter={props.center}
        />
      )}
      {hasFilter(LocationFilterType.Speed) && (
        <SpeedFilterModal
          filterKey={props.configuration.filterKey}
          // average={props.avgSpeed}
        />
      )}
      {hasFilter(LocationFilterType.AvgSpeed) && (
        <AverageSpeedFilterModal filterKey={props.configuration.filterKey} />
      )}
      {hasFilter(LocationFilterType.Altitude) && (
        <AltitudeFilterModal
          filterKey={props.configuration.filterKey}
          // average={props.altitude}
        />
      )}
      {hasFilter(LocationFilterType.Duration) && (
        <DurationFilterModal filterKey={props.configuration.filterKey} />
      )}
    </>
  );
}
