import { ref } from "yup";
import { object, ObjectSchema, string } from "yup";
import { RegisterFormValues } from "./RegisterFormValues";

export const useRegisterFormValidationSchema = () => {
  const validationSchema: ObjectSchema<RegisterFormValues> = object({
    email: string()
      .email("generic.email.invalid")
      .required("generic.email.required"),
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
