export type LocationModel = {
  id: number,
  assetId: number,
  latitude: number,
  longitude: number,
  dateTime: Date,
  speed: number,
  heading: number,
  altitude: number,
  satellites: number,
  hdop: number
};

export const DefaultLocationModel : LocationModel = {
  id: 0,
  assetId: 0,
  latitude: 0,
  longitude: 0,
  dateTime: new Date(),
  speed: 0,
  heading: 0,
  altitude: 0,
  satellites: 0,
  hdop: 0
};