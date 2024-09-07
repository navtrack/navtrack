import { LatLongModel } from "@navtrack/shared/api/model/generated";

export const DEFAULT_MAP_CENTER: LatLongModel = {
  latitude: 46.770439,
  longitude: 23.591423
};

export const DEFAULT_MAP_ZOOM = 15;
export const GEOFENCE_CIRCLE_DEFAULT_MAP_ZOOM = 14;
export const GEOFENCE_CIRCLE_MAX_RADIUS_METERS = 1000;

export const AUTHENTICATION = {
  CLIENT_ID: "navtrack.web"
};
