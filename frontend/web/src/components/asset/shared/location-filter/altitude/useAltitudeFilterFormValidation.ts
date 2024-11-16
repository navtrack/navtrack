import { object, ObjectSchema, string } from "yup";
import { AltitudeFilterFormValues } from "../locationFilterTypes";

export const useAltitudeFilterFormValidation = () => {
  const validationSchema: ObjectSchema<AltitudeFilterFormValues> = object({
    minAltitude: string().matches(/^[0-9]+$/, "generic.number.required"),
    maxAltitude: string().matches(/^[0-9]+$/, "generic.number.required")
  }).defined();

  return validationSchema;
};
