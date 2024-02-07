import { useIntl } from "react-intl";
import { object, ObjectSchema, string } from "yup";
import { AltitudeFilterFormValues } from "../locationFilterTypes";

export const useAltitudeFilterFormValidation = () => {
  const intl = useIntl();

  const validationSchema: ObjectSchema<AltitudeFilterFormValues> = object({
    minAltitude: string().matches(
      /^[0-9]+$/,
      intl.formatMessage({ id: "generic.number.required" })
    ),
    maxAltitude: string().matches(
      /^[0-9]+$/,
      intl.formatMessage({ id: "generic.number.required" })
    )
  }).defined();

  return validationSchema;
};
