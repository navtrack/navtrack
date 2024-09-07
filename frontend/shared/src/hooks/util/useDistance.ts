import { useCallback } from "react";
import { useCurrentUnits } from "./useCurrentUnits";
import { UnitsType } from "../../api/model/generated";

function convertKphToMph(speed: number) {
  return Math.round((speed / 0.621371192) * 100) / 100;
}

export function useDistance() {
  const units = useCurrentUnits();

  const convertToSelectedUnitsType = useCallback(
    (distance?: number) => {
      if (distance !== undefined) {
        return units.unitsType === UnitsType.Metric
          ? distance
          : Math.round(distance * 3.2808399 * 100) / 100;
      }

      return 0;
    },
    [units.unitsType]
  );

  const showSpeed = useCallback(
    (speed?: number | null) => {
      const convertedSpeed =
        units.unitsType === UnitsType.Imperial
          ? convertKphToMph(speed ?? 0)
          : speed ?? 0;

      return `${Math.round(convertedSpeed)} ${units.speed}`;
    },
    [units.speed, units.unitsType]
  );

  const showAltitude = useCallback(
    (altitude?: number | null) =>
      altitude !== undefined && altitude !== null
        ? `${Math.round(convertToSelectedUnitsType(altitude))} ${units.length}`
        : "-",
    [convertToSelectedUnitsType, units.length]
  );

  const showHeading = useCallback(
    (heading?: number | null) =>
      heading !== undefined && heading !== null ? `${heading}°` : "",
    []
  );

  return { showSpeed, showAltitude, showHeading };
}
