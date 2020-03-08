import { LocationModel, DefaultLocationModel } from "../location/LocationModel";

export type AssetModel = {
  id: number;
  name: string;
  deviceId: number;
  deviceType: string;
  location: LocationModel;
};

export const DefaultAssetModel: AssetModel = {
  id: 0,
  name: "",
  deviceId: 0,
  deviceType: "",
  location: DefaultLocationModel
};
