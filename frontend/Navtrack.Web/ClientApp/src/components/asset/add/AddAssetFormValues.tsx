export type AddAssetFormValues = {
  name: string;
  deviceTypeId: string;
  serialNumber: string;
};

export const DefaultAddAssetFormValues: AddAssetFormValues = {
  name: "",
  deviceTypeId: "",
  serialNumber: ""
};
