import { NumberFilterType } from "./NumberFilterType";
import { ComparisonType } from "./ComparisonType";

export type AltitudeFilterModel = {
  single: number;
  min: number;
  max: number;
  numberFilterType: NumberFilterType;
  enabled: boolean;
  comparisonType: ComparisonType;
};

export const DefaultAltitudeFilterModel: AltitudeFilterModel = {
  single: 0,
  min: 0,
  max: 1000,
  numberFilterType: NumberFilterType.Single,
  enabled: false,
  comparisonType: ComparisonType.GreaterThan
};

export const altitudeFilterToString = (filter: AltitudeFilterModel) => {
  const comparisonText = {
    [ComparisonType.GreaterThan]: ">",
    [ComparisonType.Equals]: "=",
    [ComparisonType.LessThan]: "<"
  };

  if (filter.numberFilterType === NumberFilterType.Single) {
    return `${comparisonText[filter.comparisonType]} ${filter.single} m`;
  } else {
    return `${filter.min} m - ${filter.max} m`;
  }
};
