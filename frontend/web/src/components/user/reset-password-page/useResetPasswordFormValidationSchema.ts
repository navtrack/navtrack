import { object, ref, ObjectSchema, string } from "yup";
import { ResetPasswordFormValues } from "./ResetPasswordFormValues";

export const useResetPasswordFormValidationSchema = () => {
  const validationSchema: ObjectSchema<ResetPasswordFormValues> = object({
    password: string()
      .required("generic.password.required")
      .min(8, "generic.password.requirements.length"),
    confirmPassword: string()
      .required("generic.confirm-password.required")
      .min(8, "generic.password.requirements.length")
      .oneOf([ref("password")], "generic.confirm-password.requirements.match")
  }).defined();

  return validationSchema;
};
