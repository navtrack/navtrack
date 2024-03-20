import { CircleGeofence } from "../../../ui/map/geofence/GeofenceCircle";
import { LongLat } from "../../../ui/map/mapTypes";

export enum LocationFilterType {
  Date,
  Geofence,
  Speed,
  Altitude,
  Duration
}

export enum DateRange {
  Last7Days,
  Last14Days,
  Last28Days,
  Last3Months,
  Last6Months,
  Last12Months,
  Custom
}

type FilterOptions = {
  enabled: boolean;
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
    center: LongLat;
    zoom: number;
  };
};

export type SpeedFilter = FilterOptions & {
  minSpeed?: number;
  maxSpeed?: number;
};

export type SpeedFilterFormValues = {
  minSpeed: string;
  maxSpeed: string;
};

export type AltitudeFilter = FilterOptions & {
  minAltitude?: number;
  maxAltitude?: number;
};

export type AltitudeFilterFormValues = {
  minAltitude?: string;
  maxAltitude?: string;
};

export type DurationFilter = FilterOptions & {
  minDuration?: number;
  maxDuration?: number;
};

export type DurationFilterFormValues = {
  minDuration: string;
  maxDuration: string;
};
