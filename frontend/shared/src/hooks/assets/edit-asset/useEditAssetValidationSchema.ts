import { object, ObjectSchema, string } from "yup";
import { EditAssetFormValues } from "./EditAssetFormValues";

export const useEditAssetValidationSchema = () => {
  const validationSchema: ObjectSchema<EditAssetFormValues> = object({
    name: string().required("generic.name.required"),
    deviceTypeId: string().required("generic.device-type.required"),
    serialNumber: string().required("generic.serial-number.required")
  }).defined();

  return validationSchema;
};
