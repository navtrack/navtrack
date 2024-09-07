import { useCallback } from "react";
import { useCurrentUnits } from "./useCurrentUnits";
import { UnitsType } from "../../api/model/generated";

export function useShow() {
  const units = useCurrentUnits();

  const showDistance = useCallback(
    (distance?: number | null, divide: boolean | undefined = undefined) => {
      if (distance === undefined || distance === null) {
        return undefined;
      }

      const convertedDistance =
        units.unitsType === UnitsType.Metric ? distance : distance * 3.2808399;

      const couldBeDivded = convertedDistance > 1000;
      const shouldBeDivided =
        (couldBeDivded && divide === undefined) || divide === true;

      const dividedDistance = shouldBeDivided
        ? units.unitsType === UnitsType.Metric
          ? convertedDistance / 1000
          : convertedDistance / 5280
        : convertedDistance;

      const roundedDistance = Math.round(dividedDistance * 100) / 100;

      return `${roundedDistance} ${
        shouldBeDivided ? units.lengthK : units.length
      }`;
    },
    [units.length, units.lengthK, units.unitsType]
  );

  const showDuration = useCallback((minutes?: number | null) => {
    if (minutes === undefined || minutes === null) {
      return undefined;
    }

    return minutes > 60
      ? `${Math.floor(minutes / 60)} h ${Math.round(minutes % 60)} m`
      : `${Math.round(minutes)} m`;
  }, []);

  const showVolume = useCallback(
    (volume?: number | null) => {
      if (volume === undefined || volume === null) {
        return undefined;
      }

      const convertedVolume =
        units.unitsType === UnitsType.Imperial ? volume / 3.785 : volume ?? 0;

      const roundedVolume = Math.round(convertedVolume * 100) / 100;

      return `${roundedVolume} ${units.volume}`;
    },
    [units.unitsType, units.volume]
  );

  return {
    distance: showDistance,
    duration: showDuration,
    volume: showVolume
  };
}
