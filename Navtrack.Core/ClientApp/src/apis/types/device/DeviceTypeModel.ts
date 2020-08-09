import { ProtocolModel, DefaultProtocolModel } from "../protocol/ProtocolModel";

export type DeviceTypeModel = {
  id: number;
  manufacturer: string;
  model: string;
  displayName: string;
  protocol: ProtocolModel;
};

export const DefaultDeviceTypeModel : DeviceTypeModel = {
  id: 0,
  manufacturer: "",
  model: "",
  displayName: "",
  protocol: DefaultProtocolModel
};
