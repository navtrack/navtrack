import { format, subDays } from "date-fns";
import { atom } from "jotai";
import { atomFamily, atomWithReset } from "jotai/utils";
import {
  AltitudeFilter,
  CircleGeofenceFilter,
  DateFilter,
  DateRange,
  DurationFilter,
  LocationFilterType,
  SpeedFilter
} from "./locationFilterTypes";
import { dateOptions } from "./date/dateOptions";

export const altitudeFilterAtom = atomFamily(() =>
  atomWithReset<AltitudeFilter>({
    open: false,
    enabled: false
  })
);

export const durationFilterAtom = atomFamily(() =>
  atomWithReset<DurationFilter>({
    open: false,
    enabled: false
  })
);

export const speedFilterAtom = atomFamily(() =>
  atomWithReset<SpeedFilter>({
    open: false,
    enabled: false
  })
);

export const geofenceFilterAtom = atomFamily(() =>
  atomWithReset<CircleGeofenceFilter>({
    open: false,
    enabled: false
  })
);

export const dateFilterAtom = atomFamily(() =>
  atomWithReset<DateFilter>({
    startDate: dateOptions[0].startDate!,
    endDate: dateOptions[0].endDate!,
    range: DateRange.ThisWeek,
    open: false
  })
);

export const filtersEnabledSelector = atomFamily((key: string) =>
  atom((get) => {
    return {
      [LocationFilterType.Altitude]: get(altitudeFilterAtom(key)).enabled,
      [LocationFilterType.Speed]: get(speedFilterAtom(key)).enabled,
      [LocationFilterType.Geofence]: get(geofenceFilterAtom(key)).enabled,
      [LocationFilterType.Duration]: get(durationFilterAtom(key)).enabled,
      all:
        get(altitudeFilterAtom(key)).enabled &&
        get(speedFilterAtom(key)).enabled &&
        get(geofenceFilterAtom(key)).enabled &&
        get(durationFilterAtom(key)).enabled
    };
  })
);

export const locationFiltersSelector = atomFamily((key: string) =>
  atom((get) => {
    const altitude = get(altitudeFilterAtom(key));
    const duration = get(durationFilterAtom(key));
    const speed = get(speedFilterAtom(key));
    const geofence = get(geofenceFilterAtom(key));
    const date = get(dateFilterAtom(key));

    return {
      startDate: date.startDate
        ? format(date.startDate, "yyyy-MM-dd")
        : undefined,
      endDate: date.endDate ? format(date.endDate, "yyyy-MM-dd") : undefined,
      minAltitude: altitude?.enabled ? altitude.minAltitude : undefined,
      maxAltitude: altitude?.enabled ? altitude.maxAltitude : undefined,
      minDuration: duration?.enabled ? duration.minDuration : undefined,
      maxDuration: duration?.enabled ? duration.maxDuration : undefined,
      minSpeed: speed?.enabled ? speed.minSpeed : undefined,
      maxSpeed: speed?.enabled ? speed.maxSpeed : undefined,
      latitude: geofence?.enabled ? geofence.geofence?.latitude : undefined,
      longitude: geofence?.enabled ? geofence.geofence?.longitude : undefined,
      radius: geofence?.enabled ? geofence.geofence?.radius : undefined
    };
  })
);
