import { useCallback } from "react";
import { useCurrentUnits } from "./useCurrentUnits";
import { UnitsType } from "../../api/model/generated";
import { useIntl } from "react-intl";
import { format, parseISO } from "date-fns";

export function useShow() {
  const intl = useIntl();
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

  const showFuelConsumption = useCallback(
    (fuel?: number | null) => {
      if (fuel === undefined || fuel === null) {
        return undefined;
      }

      const convertedVolume =
        units.unitsType === UnitsType.Imperial ? 235.215 / fuel : fuel ?? 0;

      const roundedVolume = Math.round(convertedVolume * 100) / 100;

      return `${roundedVolume} ${units.fuelConsumption}`;
    },
    [units.fuelConsumption, units.unitsType]
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

  return {
    distance: showDistance,
    duration: showDuration,
    volume: showVolume,
    date: showDate,
    time: showTime,
    dateTime: showDateTime,
    showFuelConsumption
  };
}
