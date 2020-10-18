import { LocationFilterModel } from "./types/LocationFilterModel";
import { NumberFilterType } from "./types/NumberFilterType";
import { ComparisonType } from "./types/ComparisonType";
import { LocationFilterRequestModel } from "../../../apis/types/asset/LocationFilterRequestModel";

export function MapToLocationHistoryRequestModel(
  locationFilter: LocationFilterModel,
  assetId: number
) {
  let filter: LocationFilterRequestModel = {
    startDate:
      locationFilter.date.enabled && locationFilter.date.startDate !== undefined
        ? locationFilter.date.startDate.toISOString()
        : undefined,
    endDate:
      locationFilter.date.enabled && locationFilter.date.endDate !== undefined
        ? locationFilter.date.endDate.toISOString()
        : undefined,
    minAltitude: getMin({ ...locationFilter.altitude }),
    maxAltitude: getMax({ ...locationFilter.altitude }),
    minSpeed: getMin({ ...locationFilter.speed }),
    maxSpeed: getMax({ ...locationFilter.speed })
  };

  return filter;
}

const getMin = (props: {
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

const getMax = (props: {
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
