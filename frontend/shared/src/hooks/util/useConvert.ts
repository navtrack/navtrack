import { useCallback } from "react";
import { useCurrentUnits } from "./useCurrentUnits";
import { UnitsType } from "../../api/model";

function convertKphToMph(speed: number) {
  return Math.round((speed / 0.621371192) * 100) / 100;
}

export function useConvert() {
  const units = useCurrentUnits();

  const distance = useCallback(
    (distance?: number | null, divide: boolean | undefined = undefined) => {
      if (distance === undefined || distance === null) {
        return undefined;
      }

      const convertedDistance =
        units.unitsType === UnitsType.Metric ? distance : distance * 3.2808399;

      const couldBeDivided = convertedDistance > 1000;
      const shouldBeDivided =
        (couldBeDivided && divide === undefined) || divide === true;

      const dividedDistance = shouldBeDivided
        ? units.unitsType === UnitsType.Metric
          ? convertedDistance / 1000
          : convertedDistance / 5280
        : convertedDistance;

      const roundedDistance = Math.round(dividedDistance * 100) / 100;

      return roundedDistance;
    },
    [units.unitsType]
  );

  const durationToHours = useCallback((seconds?: number | null) => {
    if (seconds === undefined || seconds === null) {
      return undefined;
    }

    return Math.floor((seconds / 60 / 60) * 100) / 100;
  }, []);

  const durationToMinutes = useCallback((seconds?: number | null) => {
    if (seconds === undefined || seconds === null) {
      return undefined;
    }

    return Math.round(seconds / 60);
  }, []);

  const fuelConsumption = useCallback(
    (fuel?: number | null) => {
      if (fuel === undefined || fuel === null) {
        return undefined;
      }

      const convertedVolume =
        units.unitsType === UnitsType.Imperial ? 235.215 / fuel : (fuel ?? 0);

      const roundedVolume = Math.round(convertedVolume * 100) / 100;

      return roundedVolume;
    },
    [units.unitsType]
  );

  const speed = useCallback(
    (speed?: number | null) => {
      const convertedSpeed =
        units.unitsType === UnitsType.Imperial
          ? convertKphToMph(speed ?? 0)
          : (speed ?? 0);

      return Math.round(convertedSpeed);
    },
    [units.unitsType]
  );

  return {
    distance,
    speed,
    fuelConsumption,
    durationToHours,
    durationToMinutes
  };
}
