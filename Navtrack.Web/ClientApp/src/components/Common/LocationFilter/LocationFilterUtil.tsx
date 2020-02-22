import { LocationFilterModel } from "./Models/LocationFilterModel";
import { NumberFilterType } from "./Models/NumberFilterType";
import { ComparisonType } from "./Models/ComparisonType";
import { LocationHistoryRequestModel } from "services/Api/Model/LocationHistoryRequestModel";

export function MapToLocationHistoryRequestModel(locationFilter: LocationFilterModel, assetId: number) {
  let filter: LocationHistoryRequestModel = {
    assetId: assetId,
    startDate: locationFilter.date.startDate.toISOString(),
    endDate: locationFilter.date.endDate.toISOString(),
    startAltitude: getStart({ ...locationFilter.altitude }),
    endAltitude: getEnd({ ...locationFilter.altitude }),
    startSpeed: getStart({ ...locationFilter.speed }),
    endSpeed: getEnd({ ...locationFilter.altitude })
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
      if (props.comparisonType === ComparisonType.GreaterThan || props.comparisonType === ComparisonType.Equals) {
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
      if (props.comparisonType === ComparisonType.Equals || props.comparisonType === ComparisonType.LessThan) {
        return props.single;
      }
    } else {
      return props.max;
    }
  }

  return undefined;
};
