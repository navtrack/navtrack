import { useIntl } from "react-intl";
import { number, object, SchemaOf } from "yup";
import { DurationFilterFormValues } from "../types";

export const useDurationFilterFormValuesValidation = () => {
  const intl = useIntl();

  const validationSchema: SchemaOf<DurationFilterFormValues> = object()
    .shape({
      minDuration: number()
        .typeError(intl.formatMessage({ id: "generic.number.required" }))
        .optional(),
      maxDuration: number()
        .typeError(intl.formatMessage({ id: "generic.number.required" }))
        .optional()
    })
    .defined();

  return validationSchema;
};
