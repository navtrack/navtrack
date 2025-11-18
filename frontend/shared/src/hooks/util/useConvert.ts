import { useCallback } from "react";
import { useCurrentUnits } from "./useCurrentUnits";
import { UnitsType } from "../../api/model";
import { useIntl } from "react-intl";
import { format, parseISO } from "date-fns";

function convertKphToMph(speed: number) {
  return Math.round((speed / 0.621371192) * 100) / 100;
}

export function useConvert() {
  const intl = useIntl();
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

  const showVolume = useCallback(
    (volume?: number | null) => {
      if (volume === undefined || volume === null) {
        return undefined;
      }

      const convertedVolume =
        units.unitsType === UnitsType.Imperial ? volume / 3.785 : (volume ?? 0);

      const roundedVolume = Math.round(convertedVolume * 100) / 100;

      return `${roundedVolume} ${units.volume}`;
    },
    [units.unitsType, units.volume]
  );

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

  const showDate = useCallback(
    (
      date?: string | Date | null,
      customFormat?: string,
      customMessage?: string
    ): string => {
      if (date !== undefined && date !== null) {
        return format(
          typeof date === "string" ? parseISO(date) : date,
          customFormat ?? "yyyy-MM-dd"
        );
      }

      return customMessage ?? intl.formatMessage({ id: "generic.na" });
    },
    [intl]
  );

  const showTime = useCallback(
    (date?: string): string =>
      date
        ? format(parseISO(date), "HH:mm:ss")
        : intl.formatMessage({ id: "generic.na" }),
    [intl]
  );

  const showDateTime = useCallback(
    (date?: string): string =>
      date
        ? `${showDate(date)} ${showTime(date)}`
        : intl.formatMessage({ id: "generic.na" }),
    [intl, showDate, showTime]
  );

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

  const showCount = useCallback(
    (id: string, count?: number | null, onlyIfPositive?: boolean) => {
      if (count !== undefined && count !== null) {
        if (count > 0 || !onlyIfPositive) {
          if (count === 1) {
            return intl.formatMessage({ id: `${id}.single` }, { count });
          } else {
            return intl.formatMessage({ id: `${id}` }, { count });
          }
        }
      }
    },
    [intl]
  );

  return {
    distance,
    speed,
    fuelConsumption,
    durationToHours,
    durationToMinutes
  };
}
