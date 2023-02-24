import { object, ref, SchemaOf, string } from "yup";
import { ResetPasswordFormValues } from "./ResetPasswordFormValues";

export const useResetPasswordFormValidationSchema = () => {
  const validationSchema: SchemaOf<ResetPasswordFormValues> = object({
    password: string()
      .required("generic.password.required")
      .min(8, "generic.password.requirements.length")
      .defined(),
    confirmPassword: string()
      .equals([ref("password")], "generic.confirm-password.requirements.match")
      .required("generic.confirm-password.required")
      .min(8, "generic.password.requirements.length")
      .defined(),
  }).defined();

  return validationSchema;
};
