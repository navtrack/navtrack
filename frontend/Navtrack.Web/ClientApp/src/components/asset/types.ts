export type AssetConfiguration = {
  liveTracking: LiveTracking;
};

type LiveTracking = {
  follow: boolean;
  zoom: number;
};
