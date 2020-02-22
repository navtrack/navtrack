export type CoordinatesFilterModel = {
  latitude: number;
  longitude: number;
  radius: number;
  enabled: boolean;
};

export const DefaultCoordinatesFilterModel: CoordinatesFilterModel = {
  latitude: 0,
  longitude: 1,
  radius: 100,
  enabled: false
};

export const coordinatesFilterToString = (filter: CoordinatesFilterModel) => {
  return `${filter.latitude},${filter.longitude} (${filter.radius}m)`;
};
