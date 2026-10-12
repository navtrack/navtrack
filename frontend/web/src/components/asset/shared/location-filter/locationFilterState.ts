import { format } from "date-fns";
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

export const dateFilterAtom = atomFamily(() =>
  atomWithReset<DateFilter>({
    startDate: dateOptions[0].startDate!,
    endDate: dateOptions[0].endDate!,
    range: DateRange.ThisWeek,
    open: false
  })
);

export const altitudeFilterAtom = atomFamily(() =>
  atomWithReset<AltitudeFilter>({
    open: false,
    active: false
  })
);

export const durationFilterAtom = atomFamily(() =>
  atomWithReset<DurationFilter>({
    open: false,
    active: false
  })
);

export const speedFilterAtom = atomFamily(() =>
  atomWithReset<SpeedFilter>({
    open: false,
    active: false
  })
);
export const averageSpeedFilterAtom = atomFamily(() =>
  atomWithReset<SpeedFilter>({
    open: false,
    active: false
  })
);

export const geofenceFilterAtom = atomFamily(() =>
  atomWithReset<CircleGeofenceFilter>({
    open: false,
    active: false
  })
);

const activeFilterAtoms = [
  [LocationFilterType.Altitude, altitudeFilterAtom],
  [LocationFilterType.Speed, speedFilterAtom],
  [LocationFilterType.AvgSpeed, averageSpeedFilterAtom],
  [LocationFilterType.Geofence, geofenceFilterAtom],
  [LocationFilterType.Duration, durationFilterAtom]
] as const;

export const locationFiltersActiveListSelector = atomFamily((key: string) =>
  atom((get) => {
    const activeFilters: LocationFilterType[] = activeFilterAtoms
      .filter(([, filterAtom]) => get(filterAtom(key)).active)
      .map(([type]) => type);

    return activeFilters;
  })
);

type OpenLocationFilterInput = {
  type: LocationFilterType;
  order: number;
};

export const locationFilterOpenSelector = atomFamily((key: string) =>
  atom(null, (_, set, input: OpenLocationFilterInput) => {
    const value = {
      open: true,
      order: input.order
    };

    switch (input.type) {
      case LocationFilterType.Altitude:
        set(altitudeFilterAtom(key), (prev) => ({ ...prev, ...value }));
        break;
      case LocationFilterType.Duration:
        set(durationFilterAtom(key), (prev) => ({ ...prev, ...value }));
        break;
      case LocationFilterType.Geofence:
        set(geofenceFilterAtom(key), (prev) => ({ ...prev, ...value }));
        break;
      case LocationFilterType.Speed:
        set(speedFilterAtom(key), (prev) => ({ ...prev, ...value }));
        break;
      case LocationFilterType.AvgSpeed:
        set(averageSpeedFilterAtom(key), (prev) => ({ ...prev, ...value }));
        break;
    }
  })
);

export const locationFiltersSelector = atomFamily((key: string) =>
  atom((get) => {
    const dateFilter = get(dateFilterAtom(key));
    const durationFilter = get(durationFilterAtom(key));
    const geofenceFilter = get(geofenceFilterAtom(key));
    const speedFilter = get(speedFilterAtom(key));
    const averageSpeedFilter = get(averageSpeedFilterAtom(key));
    const altitudeFilter = get(altitudeFilterAtom(key));

    return {
      startDate: dateFilter.startDate
        ? format(dateFilter.startDate, "yyyy-MM-dd")
        : undefined,
      endDate: dateFilter.endDate
        ? format(dateFilter.endDate, "yyyy-MM-dd")
        : undefined,
      minAltitude: altitudeFilter.active
        ? altitudeFilter.minAltitude
        : undefined,
      maxAltitude: altitudeFilter.active
        ? altitudeFilter.maxAltitude
        : undefined,
      minDuration: durationFilter.active
        ? durationFilter.minDuration
        : undefined,
      maxDuration: durationFilter.active
        ? durationFilter.maxDuration
        : undefined,
      minSpeed: speedFilter.active ? speedFilter.minSpeed : undefined,
      maxSpeed: speedFilter.active ? speedFilter.maxSpeed : undefined,
      minAvgSpeed: averageSpeedFilter.active
        ? averageSpeedFilter.minSpeed
        : undefined,
      maxAvgSpeed: averageSpeedFilter.active
        ? averageSpeedFilter.maxSpeed
        : undefined,
      latitude: geofenceFilter.active
        ? geofenceFilter.geofence?.latitude
        : undefined,
      longitude: geofenceFilter.active
        ? geofenceFilter.geofence?.longitude
        : undefined,
      radius: geofenceFilter.active
        ? geofenceFilter.geofence?.radius
        : undefined
    };
  })
);
