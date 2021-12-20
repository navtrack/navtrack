import { useIntl } from "react-intl";
import { number, object, SchemaOf } from "yup";
import { AltitudeFilterFormValues } from "../types";

export const useAltitudeFilterFormValuesValidation = () => {
  const intl = useIntl();

  const validationSchema: SchemaOf<AltitudeFilterFormValues> = object()
    .shape({
      minAltitude: number()
        .typeError(intl.formatMessage({ id: "generic.number.required" }))
        .optional(),
      maxAltitude: number()
        .typeError(intl.formatMessage({ id: "generic.number.required" }))
        .optional()
    })
    .defined();

  return validationSchema;
};
