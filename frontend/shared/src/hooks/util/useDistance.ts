import { useCallback } from "react";
import { useCurrentUnits } from "./useCurrentUnits";
import { UnitsType } from "../../api/model/custom/UnitsType";

function convertMetersToKm(distance: number) {
  return Math.round((distance / 1000) * 100) / 100;
}

function convertFeetToMiles(distance: number) {
  return Math.round((distance / 5280) * 100) / 100;
}

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

  const showDistance = useCallback(
    (distance?: number) => {
      const convertedDistance = convertToSelectedUnitsType(distance);

      if (convertedDistance > 1000) {
        if (units.unitsType === UnitsType.Metric) {
          return `${convertMetersToKm(convertedDistance)} ${units.lengthK}`;
        }

        return `${convertFeetToMiles(convertedDistance)} ${units.lengthK}`;
      }

      return `${convertedDistance} ${units.length}`;
    },
    [convertToSelectedUnitsType, units.length, units.lengthK, units.unitsType]
  );

  const showAltitude = useCallback(
    (altitude?: number | null) =>
      altitude !== undefined && altitude !== null
        ? `${convertToSelectedUnitsType(altitude)} ${units.length}`
        : "",
    [convertToSelectedUnitsType, units.length]
  );

  const showHeading = useCallback(
    (heading?: number | null) =>
      heading !== undefined && heading !== null ? `${heading}°` : "",
    []
  );

  return { showSpeed, showDistance, showAltitude, showHeading };
}
