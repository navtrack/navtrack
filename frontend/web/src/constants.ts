import { LatLong } from "@navtrack/shared/api/model";

export const DEFAULT_MAP_CENTER: LatLong = {
  latitude: 46.770439,
  longitude: 23.591423
};
export const DEFAULT_MAP_ZOOM = 3;
export const DEFAULT_MAP_ZOOM_FOR_LIVE_TRACKING = 13;

export const GEOFENCE_CIRCLE_DEFAULT_MAP_ZOOM = 14;
export const GEOFENCE_CIRCLE_MAX_RADIUS_METERS = 1000;

export const AUTHENTICATION = {
  CLIENT_ID: "navtrack.web"
};

export const ZINDEX_MODAL = 1000;
export const ZINDEX_MENU = 900;
export const ZINDEX_MAP_CONTROL = 500;
