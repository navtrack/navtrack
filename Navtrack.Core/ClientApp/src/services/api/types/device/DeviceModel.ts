import { DeviceModelModel } from "./DeviceModelModel";

export type DeviceModel = {
  id: number;
  deviceId: string;
  name: string;
  deviceModelId: number;
  model?: DeviceModelModel;
};

export const DefaultDeviceModel: DeviceModel = {
  id: 0,
  deviceId: "",
  name: "",
  deviceModelId: 0
};
