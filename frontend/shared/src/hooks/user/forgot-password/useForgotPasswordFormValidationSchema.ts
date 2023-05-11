import { object, ObjectSchema, string } from "yup";
import { ForgotPasswordFormValues } from "./ForgotPasswordFormValues";

export const useForgotPasswordFormValidationSchema = () => {
  const validationSchema: ObjectSchema<ForgotPasswordFormValues> = object({
    email: string()
      .email("generic.email.invalid")
      .required("generic.email.required")
  }).defined();

  return validationSchema;
};
