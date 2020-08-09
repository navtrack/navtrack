import { LocationModel, DefaultLocationModel } from "../location/LocationModel";
import { DeviceModel, DefaultDeviceModel } from "../device/DeviceModel";

export type AssetModel = {
  id: number;
  name: string;
  activeDevice: DeviceModel;
  devices: DeviceModel[];
  location: LocationModel;
};

export const DefaultAssetModel: AssetModel = {
  id: 0,
  name: "",
  activeDevice: DefaultDeviceModel,
  location: DefaultLocationModel,
  devices: []
};
