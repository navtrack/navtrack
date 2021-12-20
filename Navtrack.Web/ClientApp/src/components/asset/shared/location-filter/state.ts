import { format, subDays } from "date-fns";
import { atomFamily, selectorFamily } from "recoil";
import {
  AltitudeFilter,
  CircleGeofenceFilter as GeofenceFilter,
  DateFilter,
  DateRange,
  DurationFilter,
  LocationFilterType,
  SpeedFilter
} from "./types";

export const altitudeFilterAtom = atomFamily<AltitudeFilter, string>({
  key: "Log:Filter:Altitude",
  default: {
    open: false,
    enabled: false
  }
});

export const durationFilterAtom = atomFamily<DurationFilter, string>({
  key: "Log:Filter:Duration",
  default: {
    open: false,
    enabled: false
  }
});

export const speedFilterAtom = atomFamily<SpeedFilter, string>({
  key: "Log:Filter:Speed",
  default: {
    open: false,
    enabled: false
  }
});

export const geofenceFilterAtom = atomFamily<GeofenceFilter, string>({
  key: "Log:Filter:Geofence",
  default: {
    open: false,
    enabled: false
  }
});

export const dateFilterAtom = atomFamily<DateFilter, string>({
  key: "Log:Filter:Date",
  default: {
    startDate: subDays(new Date(), 28),
    endDate: undefined,
    range: DateRange.Last28Days,
    open: false
  }
});

export const filtersEnabledSelector = selectorFamily({
  key: "Log:Filter:Enabled",
  get:
    (key: string) =>
    ({ get }) => {
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
    }
});

export const locationFiltersSelector = selectorFamily({
  key: "Log:Filters",
  get:
    (key: string) =>
    ({ get }) => {
      const altitude = get(altitudeFilterAtom(key));
      const duration = get(durationFilterAtom(key));
      const speed = get(speedFilterAtom(key));
      const geofence = get(geofenceFilterAtom(key));
      const date = get(dateFilterAtom(key));

      return {
        startDate: date.startDate ? format(date.startDate, "yyyy-MM-dd") : undefined,
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
    }
});
