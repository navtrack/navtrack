import { ObjectSchema, object, string } from "yup";

export type EditAssetFormValues = {
  name: string;
  deviceTypeId: string;
  serialNumber: string;
};

export const useEditAssetValidationSchema = () => {
  const validationSchema: ObjectSchema<EditAssetFormValues> = object({
    name: string().required("name.required"),
    deviceTypeId: string().required("device-type.required"),
    serialNumber: string().required("serial-number.required")
  }).defined();

  return validationSchema;
};
