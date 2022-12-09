import { useCallback } from "react";
import { UnitsType } from "../../api/model/custom/UnitsType";
import { useCurrentUnits } from "./useCurrentUnits";

export const useDistance = () => {
  const units = useCurrentUnits();

  const showSpeed = useCallback(
    (speed?: number | null) =>
      speed !== undefined && speed !== null
        ? `${Math.round(speed)} ${units.speed}`
        : "",
    [units.speed]
  );

  const showDistance = useCallback(
    (distance?: number) => {
      if (distance !== undefined) {
        if (distance > 1000) {
          if (units.unitsType === UnitsType.Metric) {
            return `${Math.round((distance / 1000) * 100) / 100} ${
              units.lengthK
            }`;
          }

          return `${Math.round((distance / 5280) * 100) / 100} ${
            units.lengthK
          }`;
        }

        return `${distance} ${units.length}`;
      }

      return "";
    },
    [units.length, units.lengthK, units.unitsType]
  );

  const showAltitude = useCallback(
    (altitude?: number | null) =>
      altitude !== undefined && altitude !== null
        ? `${altitude} ${units.length}`
        : "",
    [units.length]
  );

  const showHeading = useCallback(
    (heading?: number | null) =>
      heading !== undefined && heading !== null ? `${heading}°` : "",
    []
  );

  return { showSpeed, showDistance, showAltitude, showHeading };
};
