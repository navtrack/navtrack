export type AddAssetRequestModel = {
  name: string;
  deviceTypeId: number;
  deviceId: string;
};

export const DefaultAddAssetRequestModel = {
  name: "",
  deviceTypeId: 0,
  deviceId: ""
};
