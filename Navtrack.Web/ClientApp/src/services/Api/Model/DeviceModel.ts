export type DeviceModel = {
  id: number,
  imei: string,
  name: string,
  type: string,
  deviceTypeId: number
};

export const DefaultDeviceModel : DeviceModel = {
  id: 0,
  imei: "",
  name: "",
  type: "",
  deviceTypeId: 0
};