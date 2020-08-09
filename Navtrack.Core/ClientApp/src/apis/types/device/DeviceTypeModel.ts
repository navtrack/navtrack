export type DeviceTypeModel = {
  id: number;
  manufacturer: string;
  model: string;
  displayName: string;
  protocol: string;
  port: number;
};

export const DefaultDeviceTypeModel = {
  id: 0,
  manufacturer: "",
  model: "",
  displayName: "",
  protocol: "",
  port: 0
};
