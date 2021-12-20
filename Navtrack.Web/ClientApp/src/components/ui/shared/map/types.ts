export type LatLng = {
  latitude: number;
  longitude: number;
};

export type CircleGeofence = {
  latitude: number;
  longitude: number;
  radius: number;
};

export interface IGeofenceCircle {
  geofence?: CircleGeofence;
  onChange?: (geofence: CircleGeofence) => void;
}
