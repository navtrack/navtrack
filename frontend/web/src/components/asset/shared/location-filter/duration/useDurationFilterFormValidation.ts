import { object, ObjectSchema, string } from "yup";
import { DurationFilterFormValues } from "../locationFilterTypes";

export const useDurationFilterFormValidation = () => {
  const validationSchema: ObjectSchema<DurationFilterFormValues> = object({
    minDuration: string().matches(/^[0-9]+$/, "generic.number.required"),
    maxDuration: string().matches(/^[0-9]+$/, "generic.number.required")
  }).defined();

  return validationSchema;
};
