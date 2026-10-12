import { FormikHelpers } from "formik";
import { ValidationProblemDetails } from "../api/model";
import { toCamelCase } from "./string";

export const mapErrors = <T>(
  error: ValidationProblemDetails,
  formikHelpers: FormikHelpers<T>
) => {
  formikHelpers.setSubmitting(false);

  if (error.errors !== undefined && error.errors !== null) {
    Object.keys(error.errors).forEach((x) => {
      formikHelpers.setFieldError(toCamelCase(x), error.errors![x][0]);
    });
  }

  if (!hasErrors(error)) {
    formikHelpers.setStatus({
      code: error.code,
      message: error.detail ?? error.title
    });
  }
};

function hasErrors(error: ValidationProblemDetails) {
  return (
    error.errors !== undefined &&
    error.errors !== null &&
    Object.keys(error.errors).length !== 0
  );
}
