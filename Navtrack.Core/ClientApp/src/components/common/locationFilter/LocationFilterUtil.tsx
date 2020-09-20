import { LocationFilterModel } from "./types/LocationFilterModel";
import { NumberFilterType } from "./types/NumberFilterType";
import { ComparisonType } from "./types/ComparisonType";
import { LocationHistoryRequestModel } from "../../../apis/types/location/LocationHistoryRequestModel";

export function MapToLocationHistoryRequestModel(
  locationFilter: LocationFilterModel,
  assetId: number
) {
  let filter: LocationHistoryRequestModel = {
    startDate: locationFilter.date.startDate.toISOString(),
    endDate: locationFilter.date.endDate.toISOString(),
    startAltitude: getStart({ ...locationFilter.altitude }),
    endAltitude: getEnd({ ...locationFilter.altitude }),
    startSpeed: getStart({ ...locationFilter.speed }),
    endSpeed: getEnd({ ...locationFilter.speed })
  };

  return filter;
}

const getStart = (props: {
  enabled: boolean;
  numberFilterType: NumberFilterType;
  comparisonType: ComparisonType;
  single: number;
  min: number;
}) => {
  if (props.enabled) {
    if (props.numberFilterType === NumberFilterType.Single) {
      if (
        props.comparisonType === ComparisonType.GreaterThan ||
        props.comparisonType === ComparisonType.Equals
      ) {
        return props.single;
      }
    } else {
      return props.min;
    }
  }

  return undefined;
};

const getEnd = (props: {
  enabled: boolean;
  numberFilterType: NumberFilterType;
  comparisonType: ComparisonType;
  single: number;
  max: number;
}) => {
  if (props.enabled) {
    if (props.numberFilterType === NumberFilterType.Single) {
      if (
        props.comparisonType === ComparisonType.Equals ||
        props.comparisonType === ComparisonType.LessThan
      ) {
        return props.single;
      }
    } else {
      return props.max;
    }
  }

  return undefined;
};
