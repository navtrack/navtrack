import { useIntl } from "react-intl";
import { object, ObjectSchema, string } from "yup";
import { DurationFilterFormValues } from "../types";

export const useDurationFilterFormValuesValidation = () => {
  const intl = useIntl();

  const validationSchema: ObjectSchema<DurationFilterFormValues> = object({
    minDuration: string()
      .matches(
        /^[0-9]+$/,
        intl.formatMessage({ id: "generic.number.required" })
      )
      .required(intl.formatMessage({ id: "generic.number.required" })),
    maxDuration: string()
      .matches(
        /^[0-9]+$/,
        intl.formatMessage({ id: "generic.number.required" })
      )
      .required(intl.formatMessage({ id: "generic.number.required" }))
  }).defined();

  return validationSchema;
};
