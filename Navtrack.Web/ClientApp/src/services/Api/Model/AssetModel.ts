export type AssetModel = {
  id: number,
  name: string,
  deviceId: number,
  deviceType: string
};

export const DefaultAssetModel : AssetModel = {
  id: 0,
  name: "",
  deviceId: 0,
  deviceType: ""
};