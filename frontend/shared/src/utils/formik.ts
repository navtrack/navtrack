import { FormikHelpers } from "formik";
import { getError } from "./api";

export const mapErrors = <T>(error: any, formikHelpers: FormikHelpers<T>) => {
  const response = getError(error);

  formikHelpers.setSubmitting(false);

  if (response.errors !== undefined) {
    Object.keys(response.errors).forEach((x) => {
      formikHelpers.setFieldError(x, `${response.errors![x][0]}`);
    });
  }

  if (!response.errors?.length) {
    formikHelpers.setStatus({ code: response.code, message: response.message });
  }
};
