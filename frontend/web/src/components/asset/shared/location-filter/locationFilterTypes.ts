import { LatLong } from "@navtrack/shared/api/model";
import { CircleGeofence } from "../../../ui/map/geofence/GeofenceCircle";

export type LocationFilterPage =
  | "asset-trips"
  | "asset-log"
  | "asset-reports-distance"
  | "asset-reports-fuel-consumption"
  | "asset-reports-trips"
  | "asset-reports-working-hours"
  | "asset-reports-stops";

export enum LocationFilterType {
  Date,
  Geofence,
  Speed,
  AvgSpeed,
  Altitude,
  Duration
}

export type LocationFilterConfiguration = {
  filterKey: string;
  filters: LocationFilterType[];
};

export enum DateRange {
  ThisWeek,
  LastWeek,
  ThisMonth,
  LastMonth,
  Custom
}

export type LocationFilter = {
  [LocationFilterType.Date]: DateFilter;
  [LocationFilterType.Geofence]: CircleGeofenceFilter;
  [LocationFilterType.Speed]: SpeedFilter;
  [LocationFilterType.AvgSpeed]: SpeedFilter;
  [LocationFilterType.Altitude]: AltitudeFilter;
  [LocationFilterType.Duration]: DurationFilter;
};

type FilterOptions = {
  active: boolean;
  open: boolean;
  order?: number;
};

export type DateFilter = {
  startDate: Date;
  endDate: Date;
  range: DateRange;
  open: boolean;
};

export type CircleGeofenceFilter = FilterOptions & {
  geofence?: CircleGeofence;
  map?: {
    center: LatLong;
    zoom: number;
  };
};

export type SpeedFilterFormValues = {
  minSpeed?: number;
  maxSpeed?: number;
};
export type SpeedFilter = FilterOptions & SpeedFilterFormValues;

export type AltitudeFilterFormValues = {
  minAltitude?: number;
  maxAltitude?: number;
};
export type AltitudeFilter = FilterOptions & AltitudeFilterFormValues;

export type DurationFilterFormValues = {
  minDuration?: number;
  maxDuration?: number;
};
export type DurationFilter = FilterOptions & DurationFilterFormValues;
