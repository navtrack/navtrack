import { FormikHelpers } from "formik";
import { getError } from "./api";

export const mapErrors = <T>(error: any, formikHelpers: FormikHelpers<T>) => {
  const response = getError(error);

  formikHelpers.setSubmitting(false);

  response.validationErrors?.forEach((x) => {
    formikHelpers.setFieldError(x.propertyName, `${x.message}`);
  });
};
