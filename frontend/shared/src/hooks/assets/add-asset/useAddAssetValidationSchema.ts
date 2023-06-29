import { object, ObjectSchema, string } from "yup";
import { AddAssetFormValues } from "./AddAssetFormValues";

export const useAddAssetValidationSchema = () => {
  const validationSchema: ObjectSchema<AddAssetFormValues> = object({
    name: string().required("generic.name.required"),
    deviceTypeId: string().required("generic.device-type.required"),
    serialNumber: string().required("generic.serial-number.required")
  }).defined();

  return validationSchema;
};
